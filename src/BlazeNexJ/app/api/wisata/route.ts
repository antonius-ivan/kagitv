import { NextRequest } from "next/server";
import { fetchWisataApi, relayJsonResponse } from "@/app/aacomponents/server/wisata";

function buildAuthHeaders(request: NextRequest, contentType?: string) {
  return {
    ...(contentType ? { "Content-Type": contentType } : {}),
    ...(request.headers.get("authorization") ? { Authorization: request.headers.get("authorization") as string } : {})
  };
}

export async function GET(request: NextRequest) {
  const query = request.nextUrl.searchParams.toString();
  const path = query ? `/api/wisata?${query}` : "/api/wisata";
  const upstreamResponse = await fetchWisataApi(path, {
    headers: buildAuthHeaders(request)
  });

  return relayJsonResponse(upstreamResponse);
}

export async function POST(request: NextRequest) {
  const payload = await request.text();
  const upstreamResponse = await fetchWisataApi("/api/wisata", {
    method: "POST",
    headers: buildAuthHeaders(request, "application/json"),
    body: payload
  });

  return relayJsonResponse(upstreamResponse);
}