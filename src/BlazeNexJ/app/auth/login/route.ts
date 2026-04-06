import { NextRequest, NextResponse } from "next/server";
import { createAuthorizationUrl, getAppBaseUrl, setAuthState } from "@/app/aacomponents/server/auth";

export async function GET(request: NextRequest) {
  const returnTo = request.nextUrl.searchParams.get("returnTo") ?? "/salesitem";
  const signedState = await setAuthState(returnTo);
  const authorizationUrl = createAuthorizationUrl(getAppBaseUrl(request), signedState);
  return NextResponse.redirect(authorizationUrl);
}