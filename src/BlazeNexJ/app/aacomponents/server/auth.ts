import "server-only";

import { createHmac, randomUUID, timingSafeEqual } from "node:crypto";
import { cookies } from "next/headers";
import { normalizeEndpointCandidate, resolveServiceBaseUrl } from "./runtime";

export type SessionUser = {
  sub: string;
  name: string;
  email?: string;
  roles?: string[];
  street?: string;
  city?: string;
  state?: string;
  country?: string;
  zipCode?: string;
};

export type SessionPayload = {
  user: SessionUser;
  accessToken?: string;
  idToken?: string;
};

type SignedState = {
  state: string;
  returnTo: string;
};

type BaseUrlRequestLike = {
  nextUrl?: {
    origin?: string;
    protocol?: string;
  };
  headers?: {
    get(name: string): string | null;
  };
};

const sessionCookieName = "blazenexj.session";
const stateCookieName = "blazenexj.auth_state";

function getAuthSecret() {
  return process.env.AUTH_SESSION_SECRET ?? "blazenexj-dev-session-secret-change-me";
}

function getClientId() {
  return process.env.AUTH_CLIENT_ID ?? "blazenexj";
}

function getClientSecret() {
  return process.env.AUTH_CLIENT_SECRET ?? "secret";
}

function normalizeProtocol(protocol: string | null | undefined) {
  if (!protocol) {
    return null;
  }

  return protocol.endsWith(":") ? protocol.slice(0, -1) : protocol;
}

function getBaseUrlFromRequest(request?: BaseUrlRequestLike) {
  if (!request?.headers) {
    return null;
  }

  const forwardedHost = request.headers.get("x-forwarded-host");
  const host = forwardedHost ?? request.headers.get("host");

  if (!host) {
    return null;
  }

  const forwardedProto = normalizeProtocol(request.headers.get("x-forwarded-proto"));
  const requestProtocol = normalizeProtocol(request.nextUrl?.protocol);
  const protocol = forwardedProto ?? requestProtocol ?? "http";

  return `${protocol}://${host}`.replace(/\/+$/, "");
}

function sign(value: string) {
  return createHmac("sha256", getAuthSecret()).update(value).digest("base64url");
}

function encodeSignedValue<T>(payload: T) {
  const json = JSON.stringify(payload);
  const encodedPayload = Buffer.from(json, "utf8").toString("base64url");
  const encodedSignature = sign(encodedPayload);
  return `${encodedPayload}.${encodedSignature}`;
}

function decodeSignedValue<T>(value: string): T | null {
  const [encodedPayload, encodedSignature] = value.split(".");

  if (!encodedPayload || !encodedSignature) {
    return null;
  }

  const expectedSignature = sign(encodedPayload);
  const signatureBuffer = Buffer.from(encodedSignature);
  const expectedBuffer = Buffer.from(expectedSignature);

  if (signatureBuffer.length !== expectedBuffer.length) {
    return null;
  }

  if (!timingSafeEqual(signatureBuffer, expectedBuffer)) {
    return null;
  }

  try {
    const json = Buffer.from(encodedPayload, "base64url").toString("utf8");
    return JSON.parse(json) as T;
  } catch {
    return null;
  }
}

export function getIdentityUrl() {
  const identityUrl = resolveServiceBaseUrl(
    [
      process.env.Identity__Url,
      process.env.IDENTITY__URL,
      process.env["services__identity-api__http__0"],
      process.env["services__identity-api__https__0"],
      process.env["services__identity-api__http"],
      process.env["services__identity-api__https"],
      process.env["SERVICES__IDENTITY-API__HTTP__0"],
      process.env["SERVICES__IDENTITY-API__HTTPS__0"],
      process.env["SERVICES__IDENTITY-API__HTTP"],
      process.env["SERVICES__IDENTITY-API__HTTPS"],
      process.env["services__identity_api__http__0"],
      process.env["services__identity_api__https__0"],
      process.env["services__identity_api__http"],
      process.env["services__identity_api__https"],
      process.env["SERVICES__IDENTITY_API__HTTP__0"],
      process.env["SERVICES__IDENTITY_API__HTTPS__0"],
      process.env["SERVICES__IDENTITY_API__HTTP"],
      process.env["SERVICES__IDENTITY_API__HTTPS"]
    ],
    /(identity|identity-api|identity_api)/i
  );

  if (!identityUrl) {
    throw new Error("Identity__Url is not configured for BlazeNexJ.");
  }

  return identityUrl;
}

export function getAppBaseUrl(request?: BaseUrlRequestLike) {
  const configuredOrigin = normalizeEndpointCandidate(
    process.env.CallBackUrl ?? process.env.CALLBACKURL ?? process.env.CALLBACK_URL
  );

  if (configuredOrigin) {
    return configuredOrigin;
  }

  const requestBaseUrl = getBaseUrlFromRequest(request);

  if (requestBaseUrl) {
    return requestBaseUrl;
  }

  if (request?.nextUrl?.origin) {
    return request.nextUrl.origin.replace(/\/+$/, "");
  }

  throw new Error("BlazeNexJ public base URL is not configured.");
}

export async function getSession() {
  const cookieStore = await cookies();
  const rawValue = cookieStore.get(sessionCookieName)?.value;

  if (!rawValue) {
    return null;
  }

  return decodeSignedValue<SessionPayload>(rawValue);
}

export async function setSession(session: SessionPayload) {
  const cookieStore = await cookies();
  cookieStore.set(sessionCookieName, encodeSignedValue(session), {
    httpOnly: true,
    sameSite: "lax",
    secure: process.env.NODE_ENV === "production",
    path: "/",
    maxAge: 60 * 60 * 8
  });
}

export async function clearSession() {
  const cookieStore = await cookies();
  cookieStore.delete(sessionCookieName);
}

export async function setAuthState(returnTo: string) {
  const cookieStore = await cookies();
  const signedState: SignedState = {
    state: randomUUID(),
    returnTo
  };

  cookieStore.set(stateCookieName, encodeSignedValue(signedState), {
    httpOnly: true,
    sameSite: "lax",
    secure: process.env.NODE_ENV === "production",
    path: "/",
    maxAge: 60 * 10
  });

  return signedState;
}

export async function consumeAuthState() {
  const cookieStore = await cookies();
  const rawValue = cookieStore.get(stateCookieName)?.value;
  cookieStore.delete(stateCookieName);

  if (!rawValue) {
    return null;
  }

  return decodeSignedValue<SignedState>(rawValue);
}

export function createAuthorizationUrl(appBaseUrl: string, signedState: SignedState) {
  const authorizeUrl = new URL(`${getIdentityUrl()}/connect/authorize`);
  authorizeUrl.searchParams.set("client_id", getClientId());
  authorizeUrl.searchParams.set("response_type", "code");
  authorizeUrl.searchParams.set("redirect_uri", new URL("/auth/callback", appBaseUrl).toString());
  authorizeUrl.searchParams.set(
    "scope",
    "openid profile offline_access"
  );
  authorizeUrl.searchParams.set("state", signedState.state);
  return authorizeUrl;
}

export async function exchangeAuthorizationCode(code: string, appBaseUrl: string) {
  const tokenUrl = new URL(`${getIdentityUrl()}/connect/token`);
  const body = new URLSearchParams({
    grant_type: "authorization_code",
    code,
    redirect_uri: new URL("/auth/callback", appBaseUrl).toString(),
    client_id: getClientId(),
    client_secret: getClientSecret()
  });

  const response = await fetch(tokenUrl, {
    method: "POST",
    headers: {
      "Content-Type": "application/x-www-form-urlencoded"
    },
    body
  });

  if (!response.ok) {
    throw new Error(`Token exchange failed: ${response.status}`);
  }

  return (await response.json()) as {
    access_token: string;
    id_token?: string;
  };
}

export async function fetchUserInfo(accessToken: string) {
  const userInfoUrl = new URL(`${getIdentityUrl()}/connect/userinfo`);
  const response = await fetch(userInfoUrl, {
    headers: {
      Authorization: `Bearer ${accessToken}`
    },
    cache: "no-store"
  });

  if (!response.ok) {
    throw new Error(`Userinfo request failed: ${response.status}`);
  }

  const payload = (await response.json()) as {
    sub: string;
    name?: string;
    preferred_username?: string;
    email?: string;
    role?: string | string[];
    address_street?: string;
    address_city?: string;
    address_state?: string;
    address_country?: string;
    address_zip_code?: string;
  };

  const roles = Array.isArray(payload.role)
    ? payload.role
    : payload.role
      ? [payload.role]
      : [];

  return {
    sub: payload.sub,
    name: payload.name ?? payload.preferred_username ?? payload.sub,
    email: payload.email,
    roles,
    street: payload.address_street,
    city: payload.address_city,
    state: payload.address_state,
    country: payload.address_country,
    zipCode: payload.address_zip_code
  } satisfies SessionUser;
}