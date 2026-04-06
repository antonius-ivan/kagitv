import { NextRequest, NextResponse } from "next/server";
import {
  consumeAuthState,
  exchangeAuthorizationCode,
  fetchUserInfo,
  getAppBaseUrl,
  setSession
} from "@/app/aacomponents/server/auth";

export async function GET(request: NextRequest) {
  const appBaseUrl = getAppBaseUrl(request);
  const code = request.nextUrl.searchParams.get("code");
  const state = request.nextUrl.searchParams.get("state");
  const signedState = await consumeAuthState();

  if (!code || !state || !signedState || signedState.state !== state) {
    return NextResponse.redirect(new URL("/salesitem", appBaseUrl));
  }

  try {
    const tokenResponse = await exchangeAuthorizationCode(code, appBaseUrl);
    const user = await fetchUserInfo(tokenResponse.access_token);

    await setSession({
      user,
      accessToken: tokenResponse.access_token,
      idToken: tokenResponse.id_token
    });

    return NextResponse.redirect(new URL(signedState.returnTo, appBaseUrl));
  } catch {
    return NextResponse.redirect(new URL("/salesitem", appBaseUrl));
  }
}