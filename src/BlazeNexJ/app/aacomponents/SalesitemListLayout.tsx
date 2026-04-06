"use client";

import { useEffect, useState } from "react";
//import { Salesitem } from "../../../app/types";
import Link from "next/link";
import { Salesitem } from "../types";

export function SalesitemListLayout() {
  const [salesitems, setsalesitems] = useState<Salesitem[]>([]);

  useEffect(() => {
    (async () => {
      const response = await fetch(`/api/salesitems`);

      if (!response.ok) {
        setsalesitems([]);
        return;
      }

      const allsalesitems = await response.json() as Salesitem[];
      setsalesitems(Array.isArray(allsalesitems) ? allsalesitems : []);
    })();
  }, []);

  return (
    <main id="content">
      <Link href="/dashboard" className="btn-primary">
        Open Fluent UI sample pages
      </Link>
      <h1>Welcome to the salesitem app</h1>
      <p>Click on a salesitem below to learn more.</p>
      <ul>
        {salesitems.map((salesitem: Salesitem) => {
          return (
            <li key={salesitem.name}>
              <Link href={`/salesitem/${salesitem.slug}`} className="btn-primary">
                {salesitem.name}
              </Link>
            </li>
          );
        })}
      </ul>
    </main>
  );
}
