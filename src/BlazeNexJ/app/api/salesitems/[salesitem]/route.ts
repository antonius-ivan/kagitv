import { getSalesitem } from "@/app/server/salesitems";

type RouteParams = { params: Promise<{ salesitem: string }> };

export const dynamic = "force-dynamic";

export const GET = async (_request: Request, { params }: RouteParams) => {
  const { salesitem } = await params;

  if (!salesitem) {
    return Response.json({ error: "No salesitem name provided." }, { status: 400 });
  }

  try {
    const salesitemData = await getSalesitem(salesitem);

    if (!salesitemData) {
      return Response.json({ error: "No salesitem found." }, { status: 404 });
    }

    return Response.json(salesitemData);
  } catch (error) {
    return Response.json(
      {
        error: error instanceof Error ? error.message : "Unable to load sales item."
      },
      { status: 502 }
    );
  }
};