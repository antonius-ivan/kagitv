"use client";

import { Button, makeStyles, tokens } from "@fluentui/react-components";
import { useState } from "react";

const useStyles = makeStyles({
  root: {
    display: "grid",
    gap: "16px"
  },
  eyebrow: {
    color: tokens.colorPaletteRoyalBlueForeground2,
    fontSize: "12px",
    fontWeight: tokens.fontWeightSemibold,
    letterSpacing: "0.28em",
    textTransform: "uppercase"
  },
  row: {
    display: "flex",
    flexWrap: "wrap",
    alignItems: "center",
    justifyContent: "space-between",
    gap: "16px",
    border: `1px solid ${tokens.colorNeutralStroke2}`,
    borderRadius: tokens.borderRadiusXLarge,
    backgroundColor: tokens.colorNeutralBackground2,
    padding: "24px"
  },
  copy: {
    margin: 0,
    maxWidth: "560px",
    color: tokens.colorNeutralForeground3,
    lineHeight: "1.7"
  }
});

export default function FluentSampleButton() {
  const [count, setCount] = useState(0);
  const styles = useStyles();

  return (
    <div className={styles.root}>
      <span className={styles.eyebrow}>Client Component</span>
      <div className={styles.row}>
        <p className={styles.copy}>
          This Fluent UI sample button is styled with Griffel and rendered inside a route-local
          Fluent provider, so the dashboard sample can evolve independently from the Tailwind
          storefront routes.
        </p>
        <Button appearance="primary" 
            onClick={() => setCount((value) => value + 1)}>
          Clicked {count} times
        </Button>
      </div>
    </div>
  );
}
