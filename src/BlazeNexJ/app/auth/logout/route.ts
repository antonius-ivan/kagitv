import { NextRequest, NextResponse } from "next/server";
import { clearSession, getAppBaseUrl } from "@/app/aacomponents/server/auth";

export async function GET(request: NextRequest) {
  await clearSession();
  return NextResponse.redirect(new URL("/salesitem", getAppBaseUrl(request)));
}