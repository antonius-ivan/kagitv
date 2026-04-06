import { NextRequest } from "next/server";
import { fetchWisataApi, relayJsonResponse } from "@/app/aacomponents/server/wisata";

export async function GET(request: NextRequest) {
  const upstreamResponse = await fetchWisataApi("/api/wisata/report", {
    headers: request.headers.get("authorization")
      ? { Authorization: request.headers.get("authorization") as string }
      : undefined
  });

  return relayJsonResponse(upstreamResponse);
}