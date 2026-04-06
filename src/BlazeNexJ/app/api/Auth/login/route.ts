import { NextRequest, NextResponse } from "next/server";
import { fetchWisataApi, getWisataRefreshCookieName, relayJsonResponse } from "@/app/aacomponents/server/wisata";

export async function POST(request: NextRequest) {
  const payload = await request.json();

  const upstreamResponse = await fetchWisataApi("/api/Auth/login", {
    method: "POST",
    headers: {
      "Content-Type": "application/json"
    },
    body: JSON.stringify(payload)
  });

  if (!upstreamResponse.ok) {
    return relayJsonResponse(upstreamResponse);
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