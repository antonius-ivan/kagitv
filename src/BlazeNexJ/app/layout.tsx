import type { Metadata } from "next";
import "./globals.css";

export const metadata: Metadata = {
  title: "Combust",
  description: "A Next.js App Router project mixing Tailwind defaults with a route-scoped Fluent UI sample."
};

export default function RootLayout({
  children
}: Readonly<{
  children: React.ReactNode;
}>) {
  return (
    <html lang="en">
      <body>{children}</body>
    </html>
  );
}
