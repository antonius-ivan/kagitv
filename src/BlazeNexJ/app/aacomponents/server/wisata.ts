import "server-only";

import { NextResponse } from "next/server";
import { resolveServiceBaseUrl } from "./runtime";

const wisataRefreshCookieName = "testwisata.refresh";

export function getWisataRefreshCookieName() {
  return wisataRefreshCookieName;
}

export function getWisataApiBaseUrl() {
  const value = resolveServiceBaseUrl(
    [
      process.env.SALESITEM_API_URL,
      process.env["services__salesitem-api__http__0"],
      process.env["services__salesitem-api__https__0"],
      process.env["services__salesitem-api__http"],
      process.env["services__salesitem-api__https"],
      process.env["SERVICES__SALESITEM-API__HTTP__0"],
      process.env["SERVICES__SALESITEM-API__HTTPS__0"],
      process.env["SERVICES__SALESITEM-API__HTTP"],
      process.env["SERVICES__SALESITEM-API__HTTPS"],
      process.env["services__salesitem_api__http__0"],
      process.env["services__salesitem_api__https__0"],
      process.env["services__salesitem_api__http"],
      process.env["services__salesitem_api__https"],
      process.env["SERVICES__SALESITEM_API__HTTP__0"],
      process.env["SERVICES__SALESITEM_API__HTTPS__0"],
      process.env["SERVICES__SALESITEM_API__HTTP"],
      process.env["SERVICES__SALESITEM_API__HTTPS"]
    ],
    /(salesitem|salesitem-api|salesitem_api)/i
  );

  if (!value) {
    throw new Error("SalesItem API base URL is not configured for wisata routes.");
  }

  return value;
}

export async function fetchWisataApi(path: string, init?: RequestInit) {
  return fetch(new URL(path, getWisataApiBaseUrl()), {
    cache: "no-store",
    ...init,
    headers: {
      Accept: "application/json",
      ...(init?.headers ?? {})
    }
  });
}

export async function relayJsonResponse(upstreamResponse: Response) {
  if (upstreamResponse.status === 204) {
    return new NextResponse(null, { status: 204 });
  }

  const responseText = await upstreamResponse.text();

  return new NextResponse(responseText, {
    status: upstreamResponse.status,
    headers: {
      "Content-Type": upstreamResponse.headers.get("content-type") ?? "application/json"
    }
  });
}