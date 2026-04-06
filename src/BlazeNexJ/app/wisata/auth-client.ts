import type { WisataAuthPayload } from "@/app/types";

const accessTokenKey = "testwisata.accessToken";
const accessTokenExpiresAtKey = "testwisata.accessTokenExpiresAt";
const usernameKey = "testwisata.username";
const accessTokenRefreshSkewMs = 30_000;

function isAccessTokenExpired(accessTokenExpiresAt: string) {
  const expiresAt = Date.parse(accessTokenExpiresAt);

  if (!Number.isFinite(expiresAt)) {
    return true;
  }

  return expiresAt <= Date.now() + accessTokenRefreshSkewMs;
}

export function getWisataSession() {
  if (typeof window === "undefined") {
    return null;
  }

  const accessToken = window.localStorage.getItem(accessTokenKey);
  const accessTokenExpiresAt = window.localStorage.getItem(accessTokenExpiresAtKey);
  const username = window.localStorage.getItem(usernameKey);

  if (!accessToken || !accessTokenExpiresAt) {
    return null;
  }

  return {
    accessToken,
    accessTokenExpiresAt,
    username: username ?? "guest"
  };
}

export function clearWisataSession() {
  if (typeof window === "undefined") {
    return;
  }

  window.localStorage.removeItem(accessTokenKey);
  window.localStorage.removeItem(accessTokenExpiresAtKey);
  window.localStorage.removeItem(usernameKey);
}

function saveWisataSession(payload: WisataAuthPayload) {
  window.localStorage.setItem(accessTokenKey, payload.accessToken);
  window.localStorage.setItem(accessTokenExpiresAtKey, payload.accessTokenExpiresAt);
  window.localStorage.setItem(usernameKey, payload.username);
}

export async function loginWisata(username: string, password: string) {
  const response = await fetch("/api/Auth/login", {
    method: "POST",
    headers: {
      "Content-Type": "application/json"
    },
    body: JSON.stringify({ username, password })
  });

  if (!response.ok) {
    throw new Error("Username atau password tidak valid.");
  }

  const payload = await response.json() as WisataAuthPayload;
  saveWisataSession(payload);
  return payload;
}

export async function logoutWisata() {
  await fetch("/api/Auth/logout", {
    method: "POST"
  });

  clearWisataSession();
}

export async function refreshWisataAccessToken() {
  const response = await fetch("/api/Auth/refresh", {
    method: "POST"
  });

  if (!response.ok) {
    clearWisataSession();
    return null;
  }

  const payload = await response.json() as WisataAuthPayload;
  saveWisataSession(payload);
  return payload.accessToken;
}

export async function wisataApiFetch(path: string, init?: RequestInit) {
  let session = getWisataSession();

  if (!session?.accessToken) {
    throw new Error("UNAUTHORIZED");
  }

  let accessToken = session.accessToken;

  if (isAccessTokenExpired(session.accessTokenExpiresAt)) {
    const refreshedToken = await refreshWisataAccessToken();

    if (!refreshedToken) {
      throw new Error("UNAUTHORIZED");
    }

    session = getWisataSession();
    accessToken = session?.accessToken ?? refreshedToken;
  }

  const headers = new Headers(init?.headers ?? {});
  headers.set("Authorization", `Bearer ${accessToken}`);

  if (init?.body && !(init.body instanceof FormData) && !headers.has("Content-Type")) {
    headers.set("Content-Type", "application/json");
  }

  let response = await fetch(path, {
    ...init,
    headers
  });

  if (response.status !== 401) {
    return response;
  }

  const refreshedToken = await refreshWisataAccessToken();
  if (!refreshedToken) {
    throw new Error("UNAUTHORIZED");
  }

  headers.set("Authorization", `Bearer ${refreshedToken}`);
  response = await fetch(path, {
    ...init,
    headers
  });

  if (response.status === 401) {
    clearWisataSession();
    throw new Error("UNAUTHORIZED");
  }

  return response;
}