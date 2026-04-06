import { NextRequest } from "next/server";
import { fetchWisataApi, relayJsonResponse } from "@/app/aacomponents/server/wisata";

type RouteParams = {
  params: Promise<{ id: string }>;
};

function buildAuthHeaders(request: NextRequest, contentType?: string) {
  return {
    ...(contentType ? { "Content-Type": contentType } : {}),
    ...(request.headers.get("authorization") ? { Authorization: request.headers.get("authorization") as string } : {})
  };
}

export async function GET(request: NextRequest, { params }: RouteParams) {
  const { id } = await params;
  const upstreamResponse = await fetchWisataApi(`/api/wisata/${id}`, {
    headers: buildAuthHeaders(request)
  });

  return relayJsonResponse(upstreamResponse);
}

export async function PUT(request: NextRequest, { params }: RouteParams) {
  const { id } = await params;
  const payload = await request.text();
  const upstreamResponse = await fetchWisataApi(`/api/wisata/${id}`, {
    method: "PUT",
    headers: buildAuthHeaders(request, "application/json"),
    body: payload
  });

  return relayJsonResponse(upstreamResponse);
}

export async function DELETE(request: NextRequest, { params }: RouteParams) {
  const { id } = await params;
  const upstreamResponse = await fetchWisataApi(`/api/wisata/${id}`, {
    method: "DELETE",
    headers: buildAuthHeaders(request)
  });

  return relayJsonResponse(upstreamResponse);
}