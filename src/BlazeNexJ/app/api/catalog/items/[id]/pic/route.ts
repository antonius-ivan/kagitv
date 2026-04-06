import { NextRequest, NextResponse } from "next/server";
import { getCatalogItemPicture } from "@/app/aacomponents/server/catalog";

type RouteParams = {
  params: Promise<{ id: string }>;
};

export async function GET(_request: NextRequest, { params }: RouteParams) {
  const { id } = await params;
  const salesitemId = Number(id);

  if (!Number.isInteger(salesitemId) || salesitemId <= 0) {
    return NextResponse.json({ message: "Invalid catalog item id." }, { status: 400 });
  }

  try {
    const upstreamResponse = await getCatalogItemPicture(salesitemId);
    const body = await upstreamResponse.arrayBuffer();

    return new NextResponse(body, {
      status: 200,
      headers: {
        "Content-Type": upstreamResponse.headers.get("content-type") ?? "image/jpeg",
        "Cache-Control": "public, max-age=60"
      }
    });
  } catch {
    return NextResponse.json({ message: "Catalog image not found." }, { status: 404 });
  }
}