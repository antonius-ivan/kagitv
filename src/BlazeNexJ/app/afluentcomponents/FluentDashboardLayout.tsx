"use client";

import * as React from "react";
import type { MenuModule, MenuNode } from "@/app/types";
import {
  AppItem,
  Hamburger,
  NavCategory,
  NavCategoryItem,
  NavDivider,
  NavDrawer,
  NavDrawerBody,
  NavDrawerHeader,
  NavItem,
  NavSectionHeader,
  NavSubItem,
  NavSubItemGroup,
  Tooltip,
  makeStyles,
  tokens,
} from "@fluentui/react-components";
import {
  AppsList20Regular,
  Board20Filled,
  Board20Regular,
  bundleIcon,
  PersonCircle32Regular,
} from "@fluentui/react-icons";
import { usePathname } from "next/navigation";

const useClasses = makeStyles({
  root: {
    display: "flex",
    width: "100%",
    minHeight: "clamp(32rem, 72vh, 52rem)",
    overflow: "hidden",
    borderRadius: "28px",
    border: `1px solid ${tokens.colorNeutralStroke2}`,
    backgroundColor: tokens.colorNeutralBackground1,
    boxShadow: "0 18px 44px rgba(15, 23, 42, 0.08)",
    "@media screen and (max-width: 768px)": {
      minHeight: "auto",
      flexDirection: "column",
    },
  },
  navdrawer: {
    minWidth: "248px",
    maxWidth: "248px",
    borderRight: `1px solid ${tokens.colorNeutralStroke2}`,
    backgroundColor: tokens.colorNeutralBackground2,
    "@media screen and (max-width: 768px)": {
      display: "none",
    },
  },
  content: {
    flex: "1",
    minWidth: 0,
    padding: tokens.spacingHorizontalXXL,
    display: "flex",
    alignItems: "flex-start",
    justifyContent: "stretch",
    backgroundColor: tokens.colorNeutralBackground1,
    "@media screen and (max-width: 768px)": {
      padding: tokens.spacingHorizontalL,
    },
  },
  field: {
    display: "flex",
    flexDirection: "column",
    rowGap: tokens.spacingVerticalM,
    width: "100%",
    maxWidth: "100%",
  },
  heading: {
    display: "flex",
    flexDirection: "column",
    rowGap: tokens.spacingVerticalXS,
  },
  eyebrow: {
    fontSize: tokens.fontSizeBase200,
    fontWeight: tokens.fontWeightSemibold,
    letterSpacing: "0.22em",
    textTransform: "uppercase",
    color: tokens.colorBrandForeground1,
  },
  title: {
    fontSize: tokens.fontSizeHero800,
    lineHeight: tokens.lineHeightHero800,
    fontWeight: tokens.fontWeightSemibold,
    color: tokens.colorNeutralForeground1,
    margin: 0,
    "@media screen and (max-width: 768px)": {
      fontSize: tokens.fontSizeHero700,
      lineHeight: tokens.lineHeightHero700,
    },
  },
  description: {
    fontSize: tokens.fontSizeBase300,
    lineHeight: tokens.lineHeightBase400,
    color: tokens.colorNeutralForeground3,
    margin: 0,
    maxWidth: "42rem",
  },
  emptyState: {
    fontSize: tokens.fontSizeBase300,
    lineHeight: tokens.lineHeightBase400,
    color: tokens.colorNeutralForeground3,
    margin: 0,
  },
  slot: {
    width: "100%",
  },
});

const Dashboard = bundleIcon(Board20Filled, Board20Regular);
const fallbackIcon = <AppsList20Regular />;

type FluentDashboardLayoutProps = {
  modules: MenuModule[];
  eyebrow?: string;
  title?: string;
  description?: string;
  children?: React.ReactNode;
  emptyStateMessage?: string;
};

type SelectedState = {
  selectedCategoryValue?: string;
  selectedValue?: string;
};

function getItemValue(menuId: number) {
  return `menu:${menuId}`;
}

function isPathActive(pathname: string, targetPath?: string) {
  if (!targetPath) {
    return false;
  }

  return pathname === targetPath || pathname.startsWith(`${targetPath}/`);
}

function findSelectedState(modules: MenuModule[], pathname: string): SelectedState {
  for (const module of modules) {
    for (const node of module.children) {
      if (isPathActive(pathname, node.path)) {
        return { selectedValue: getItemValue(node.id) };
      }

      for (const child of flattenMenuNodes(node.children)) {
        if (isPathActive(pathname, child.path)) {
          return {
            selectedCategoryValue: getItemValue(node.id),
            selectedValue: getItemValue(child.id)
          };
        }
      }
    }
  }

  return {};
}

function flattenMenuNodes(nodes: MenuNode[], depth = 0): Array<MenuNode & { depth: number }> {
  return nodes.flatMap((node) => [
    { ...node, depth },
    ...flattenMenuNodes(node.children, depth + 1)
  ]);
}

function renderMenuNode(node: MenuNode): React.JSX.Element {
  if (node.children.length === 0) {
    return (
      <NavItem as="a" href={node.path} icon={<Dashboard />} key={node.id} value={getItemValue(node.id)}>
        {node.name}
      </NavItem>
    );
  }

  const descendants = flattenMenuNodes(node.children, 0);

  return (
    <NavCategory key={node.id} value={getItemValue(node.id)}>
      <NavCategoryItem icon={fallbackIcon}>{node.name}</NavCategoryItem>
      <NavSubItemGroup>
        {descendants.map((child) => (
          <NavSubItem href={child.path} key={child.id} value={getItemValue(child.id)}>
            {`${child.depth > 0 ? "- ".repeat(child.depth) : ""}${child.name}`}
          </NavSubItem>
        ))}
      </NavSubItemGroup>
    </NavCategory>
  );
}

export const FluentDashboardLayout = ({
  modules,
  eyebrow = "Dashboard Module",
  title = "Keep Fluent content inside the shared application scale.",
  description = "The dashboard drawer now reads the role-filtered menu tree directly, instead of injecting a second sidebar into the shared shell.",
  children,
  emptyStateMessage = "No role-mapped dashboard items are available for the current session."
}: FluentDashboardLayoutProps): React.JSX.Element => {
  const classes = useClasses();
  const pathname = usePathname();
  const { selectedCategoryValue, selectedValue } = findSelectedState(modules, pathname);

  const renderHamburgerWithToolTip = () => {
    return (
      <Tooltip content="Navigation" relationship="label">
        <Hamburger />
      </Tooltip>
    );
  };

  return (
    <div className={classes.root}>
      <NavDrawer
        tabbable={true}
        selectedValue={selectedValue}
        selectedCategoryValue={selectedCategoryValue}
        type={"inline"}
        open={true}
        className={classes.navdrawer}
      >
        <NavDrawerHeader>{renderHamburgerWithToolTip()}</NavDrawerHeader>

        <NavDrawerBody>
          <AppItem icon={<PersonCircle32Regular />} as="a">
            Kagitv ERP
          </AppItem>
          {modules.length > 0 ? (
            modules.map((module) => (
              <React.Fragment key={module.code}>
                <NavSectionHeader>{module.name}</NavSectionHeader>
                {module.children.map((node) => renderMenuNode(node))}
              </React.Fragment>
            ))
          ) : (
            <>
              <NavSectionHeader>Workspace</NavSectionHeader>
              <NavDivider />
            </>
          )}
        </NavDrawerBody>
      </NavDrawer>
      <div className={classes.content}>
        <div className={classes.field}>
          <div className={classes.heading}>
            <p className={classes.eyebrow}>{eyebrow}</p>
            <h3 className={classes.title}>{title}</h3>
            <p className={classes.description}>{description}</p>
          </div>
          {children ? <div className={classes.slot}>{children}</div> : null}
          {modules.length === 0 ? (
            <p className={classes.emptyState}>{emptyStateMessage}</p>
          ) : null}
        </div>
      </div>
    </div>
  );
};

export default FluentDashboardLayout;