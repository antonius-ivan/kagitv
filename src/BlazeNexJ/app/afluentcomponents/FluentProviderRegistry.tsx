"use client";

import {
  createDOMRenderer,
  FluentProvider,
  RendererProvider,
  SSRProvider,
  renderToStyleElements,
  webLightTheme
} from "@fluentui/react-components";
import { useServerInsertedHTML } from "next/navigation";
import { PropsWithChildren, useState } from "react";

export default function FluentProviderRegistry({ children }: PropsWithChildren) {
  const [renderer] = useState(() => createDOMRenderer());

  useServerInsertedHTML(() => <>{renderToStyleElements(renderer)}</>);

  return (
    <RendererProvider renderer={renderer}>
      <SSRProvider>
        <FluentProvider theme={webLightTheme}>{children}</FluentProvider>
      </SSRProvider>
    </RendererProvider>
  );
}
