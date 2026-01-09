//\src\combust.client\app\layouts\DashboardLayout.tsx
import * as React from "react";
import type { JSXElement } from "@fluentui/react-components";
import {
    AppItem,
    FluentProvider,
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
    webLightTheme,
} from "@fluentui/react-components";
import type { NavItemValue, OnNavItemSelectData } from "@fluentui/react-components"
import {
    Button,
    Label,
    Switch,
    Tooltip,
    makeStyles,
    tokens,
    useId,
} from "@fluentui/react-components";
import {
    Board20Filled,
    Board20Regular,
    BoxMultiple20Filled,
    BoxMultiple20Regular,
    DataArea20Filled,
    DataArea20Regular,
    DocumentBulletListMultiple20Filled,
    DocumentBulletListMultiple20Regular,
    HeartPulse20Filled,
    HeartPulse20Regular,
    MegaphoneLoud20Filled,
    MegaphoneLoud20Regular,
    NotePin20Filled,
    NotePin20Regular,
    People20Filled,
    People20Regular,
    PeopleStar20Filled,
    PeopleStar20Regular,
    Person20Filled,
    PersonLightbulb20Filled,
    PersonLightbulb20Regular,
    Person20Regular,
    PersonSearch20Filled,
    PersonSearch20Regular,
    PreviewLink20Filled,
    PreviewLink20Regular,
    bundleIcon,
    PersonCircle32Regular,
} from "@fluentui/react-icons";

const useClasses = makeStyles({
    root: {
        overflow: "hidden",
        display: "flex",
        height: "6400px",
    },
    navdrawer: {
        minWidth: "200px",
        "@media screen and (max-width: 768px)": {
            display: "none",        // hides drawer on narrow
        },
    },
    content: {
        flex: "1",
        padding: "16px",
        display: "grid",
        justifyContent: "flex-start",
        alignItems: "flex-start",
    },
    field: {
        display: "flex",
        marginTop: "4px",
        marginLeft: "8px",
        flexDirection: "column",
        gridRowGap: tokens.spacingVerticalS,
    },
});

const Person = bundleIcon(Person20Filled, Person20Regular);
const Dashboard = bundleIcon(Board20Filled, Board20Regular);
const Announcements = bundleIcon(MegaphoneLoud20Filled, MegaphoneLoud20Regular);
const EmployeeSpotlight = bundleIcon(
    PersonLightbulb20Filled,
    PersonLightbulb20Regular
);
const Search = bundleIcon(PersonSearch20Filled, PersonSearch20Regular);
const PerformanceReviews = bundleIcon(
    PreviewLink20Filled,
    PreviewLink20Regular
);
const JobPostings = bundleIcon(NotePin20Filled, NotePin20Regular);
//const Interviews = bundleIcon(People20Filled, People20Regular);
//const HealthPlans = bundleIcon(HeartPulse20Filled, HeartPulse20Regular);
//const TrainingPrograms = bundleIcon(BoxMultiple20Filled, BoxMultiple20Regular);
//const CareerDevelopment = bundleIcon(PeopleStar20Filled, PeopleStar20Regular);
const Analytics = bundleIcon(DataArea20Filled, DataArea20Regular);
const Reports = bundleIcon(
    DocumentBulletListMultiple20Filled,
    DocumentBulletListMultiple20Regular
);

// A type that represents a navItemValue and its potential children.
// An empty children array indicates a Single top level NavItem.
// A hydrated children array indicates a NavCategoryItem with children.
type NavItemValueCombo = { parent: string; children: string[] };

// This is a list of navItemValues and their potential children
// Ite exactly matches the NavDrawer in the story below
// This is how a consumer might store them in their app
const navItemValueList: NavItemValueCombo[] = [
    { parent: "101", children: [] },
    { parent: "102", children: [] },
    { parent: "103", children: [] },
    { parent: "104", children: ["105", "106"] },
    { parent: "107", children: ["108", "109", "110", "111", "112", "300"] },
    { parent: "700", children: [] },
    { parent: "701", children: [] },
];

type SelectedPage = {
    newSelectedCategory: string | undefined;
    newSelectedValue: string;
};

const getRandomPage = (): SelectedPage => {
    const randomIndex = Math.floor(Math.random() * navItemValueList.length);
    const randomItem = navItemValueList[randomIndex];

    // there are no children, so we're selecting a top level item
    if (randomItem.children.length === 0) {
        return {
            newSelectedCategory: undefined,
            newSelectedValue: randomItem.parent,
        };
    } else {
        // there are children, so we're including a category and it's child as the selectedValue
        const randomChildIndex = Math.floor(
            Math.random() * randomItem.children.length
        );
        return {
            newSelectedCategory: randomItem.parent,
            newSelectedValue: randomItem.children[randomChildIndex],
        };
    }
};

export const DashfullboardLayout = (): JSXElement => {
    const classes = useClasses();
    const [isOpen, setIsOpen] = React.useState(true);

    const multipleLabelId = useId("multiple-label");

    const [openCategories, setOpenCategories] = React.useState<NavItemValue[]>([
        "6",
        "11",
    ]);
    const [selectedCategoryValue, setSelectedCategoryValue] = React.useState<
        string | undefined
    >("6");
    const [selectedValue, setSelectedValue] = React.useState<string>("7");
    const [isMultiple, setIsMultiple] = React.useState(true);

    const handleCategoryToggle = (
        _: Event | React.SyntheticEvent<Element, Event>,
        data: OnNavItemSelectData
    ) => {
        if (data.value === undefined && data.categoryValue) {
            // we're just opening it,
            setOpenCategories([data.categoryValue as string]);
        }

        if (isMultiple) {
            // if it's already open, remove it from the list
            if (openCategories.includes(data.categoryValue as string)) {
                setOpenCategories(
                    openCategories.filter((category) => category !== data.categoryValue)
                );
            } else {
                // otherwise add it
                setOpenCategories([...openCategories, data.categoryValue as string]);
            }
        } else {
            // if it's already open, remove it from the list
            if (openCategories.includes(data.categoryValue as string)) {
                setOpenCategories([]);
            } else {
                // otherwise add it
                setOpenCategories([data.categoryValue as string]);
            }
        }
    };

    const handleItemSelect = (
        event: Event | React.SyntheticEvent<Element, Event>,
        data: OnNavItemSelectData
    ) => {
        setSelectedCategoryValue(data.categoryValue as string);
        setSelectedValue(data.value as string);
    };

    const handleNavigationClick = () => {
        const { newSelectedCategory, newSelectedValue } = getRandomPage();

        setSelectedCategoryValue(newSelectedCategory);
        setSelectedValue(newSelectedValue);
    };

    const handleMultipleChange = (
        _: Event | React.SyntheticEvent<Element, Event>,
        data: { checked: boolean }
    ) => {
        setIsMultiple(data.checked);

        if (data.checked) {
            setOpenCategories(["6", "11"]);
        } else {
            setOpenCategories(["6"]);
        }
    };

    return (
        <FluentProvider theme={webLightTheme}>
            <div className={classes.root}>
                <NavDrawer
                    // This a controlled example,
                    // so don't use these props
                    // defaultSelectedValue="7"
                    // defaultSelectedCategoryValue="6"
                    // defaultOpenCategories={['6']}
                    // multiple={isMultiple}
                    onNavCategoryItemToggle={handleCategoryToggle}
                    onNavItemSelect={handleItemSelect}
                    tabbable={true} // enables keyboard tabbing
                    openCategories={openCategories}
                    selectedValue={selectedValue}
                    selectedCategoryValue={selectedCategoryValue}
                    type={"inline"}
                    open={ true}//{isOpen}
                    className={classes.navdrawer}
                >
                    <NavDrawerHeader>
                        <Tooltip content="Close Navigation" relationship="label">
                            <Hamburger onClick={() => setIsOpen(!isOpen)} />
                        </Tooltip>
                    </NavDrawerHeader>
                    <NavDrawerBody>
                        <AppItem icon={<PersonCircle32Regular />} as="a">
                            Contoso Travel
                        </AppItem>
                        <NavItem icon={<Dashboard />} value="101">
                            Dashboard
                        </NavItem>
                        <NavCategory value="107">
                            <NavCategoryItem icon={<Person />}>
                                All Traveloka Module
                            </NavCategoryItem>
                            <NavSubItemGroup>
                                <NavSubItem href="/wisatalist" value="108">
                                    Wisata List
                                </NavSubItem>
                                <NavSubItem href="/loginform" value="109">
                                    Login Form
                                </NavSubItem>
                            </NavSubItemGroup>
                        </NavCategory>
                        <NavDivider />
                        <NavItem target="_blank" icon={<Analytics />} value="700">
                            Workforce Data
                        </NavItem>
                        <NavItem icon={<Reports />} value="701">
                            Reports
                        </NavItem>
                    </NavDrawerBody>
                </NavDrawer>
                <div className={classes.content}>
                    <div className={classes.field}>
                        <Button appearance="primary" onClick={handleNavigationClick}>
                            Navigate
                        </Button>
                        <Label id={multipleLabelId}>Categories</Label>
                        <Switch
                            checked={isMultiple}
                            onChange={(_, data) => handleMultipleChange(_, data)}
                            label={isMultiple ? "Multiple" : "Single"}
                            aria-labelledby={multipleLabelId}
                        />
                    </div>
                </div>
            </div>
        </FluentProvider>
    );
};

export default DashfullboardLayout;