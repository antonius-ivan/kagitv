import { NextRequest, NextResponse } from "next/server";
import { fetchWisataApi, getWisataRefreshCookieName, relayJsonResponse } from "@/app/aacomponents/server/wisata";

export async function POST(request: NextRequest) {
  const refreshToken = request.cookies.get(getWisataRefreshCookieName())?.value;

  if (!refreshToken) {
    return NextResponse.json({ message: "Refresh token tidak tersedia." }, { status: 401 });
  }

  const upstreamResponse = await fetchWisataApi("/api/Auth/refresh", {
    method: "POST",
    headers: {
      "Content-Type": "application/json"
    },
    body: JSON.stringify({ refreshToken })
  });

  if (!upstreamResponse.ok) {
    const response = await relayJsonResponse(upstreamResponse);
    response.cookies.delete(getWisataRefreshCookieName());
    return response;
  }

  const responsePayload = await upstreamResponse.json() as {
    accessToken: string;
    accessTokenExpiresAt: string;
    refreshToken: string;
    refreshTokenExpiresAt: string;
    username: string;
  };

  const response = NextResponse.json({
    accessToken: responsePayload.accessToken,
    accessTokenExpiresAt: responsePayload.accessTokenExpiresAt,
    username: responsePayload.username
  });

  response.cookies.set(getWisataRefreshCookieName(), responsePayload.refreshToken, {
    httpOnly: true,
    sameSite: "lax",
    secure: process.env.NODE_ENV === "production",
    path: "/",
    maxAge: 60 * 3
  });

  return response;
}