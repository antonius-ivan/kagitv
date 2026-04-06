import { createSalesitem, listSalesitems } from "@/app/server/salesitems";
import type { SalesitemDraft } from "@/app/types";
// import { createSalesitem, listSalesitems } from "@/lib/salesitems";

export const dynamic = "force-dynamic";

export async function GET() {
  try {
    return Response.json(await listSalesitems());
  } catch (error) {
    return Response.json(
      {
        error: error instanceof Error ? error.message : "Unable to load sales items."
      },
      { status: 502 }
    );
  }
}

export async function POST(request: Request) {
  try {
    const payload = (await request.json()) as Partial<SalesitemDraft>;

    const salesitem = createSalesitem({
      name: payload.name ?? "",
      description: payload.description ?? ""
    });

    return Response.json(salesitem, { status: 201 });
  } catch (error) {
    return Response.json(
      {
        error: error instanceof Error ? error.message : "Unable to create sales item."
      },
      { status: 501 }
    );
  }
}