import { NextRequest, NextResponse } from "next/server";
import { fetchWisataApi, getWisataRefreshCookieName } from "@/app/aacomponents/server/wisata";

export async function POST(request: NextRequest) {
  const refreshToken = request.cookies.get(getWisataRefreshCookieName())?.value;

  if (refreshToken) {
    await fetchWisataApi("/api/Auth/logout", {
      method: "POST",
      headers: {
        "Content-Type": "application/json"
      },
      body: JSON.stringify({ refreshToken })
    });
  }

  const response = NextResponse.json({ ok: true });
  response.cookies.delete(getWisataRefreshCookieName());
  return response;
}