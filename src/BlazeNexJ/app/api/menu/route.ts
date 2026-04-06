import { getRoleMenu } from "@/app/aacomponents/server/menu";

export const dynamic = "force-dynamic";

export async function GET() {
  try {
    return Response.json(await getRoleMenu());
  } catch (error) {
    return Response.json(
      {
        error: error instanceof Error ? error.message : "Unable to load the menu tree."
      },
      { status: 502 }
    );
  }
}