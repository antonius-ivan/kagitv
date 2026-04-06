import SharedAppFrame from "../aacomponents/layouts/SharedAppFrame";
import FluentDashPages from "../afluentcomponents/FluentDashPages";
import SharedTopBar from "../aacomponents/navigation/SharedTopBar";

export default async function DashboardPage() {
    return (
        <SharedAppFrame
            contentClassName="pt-0"
            header={
                <SharedTopBar
                    section="Dashboard"
                    title="Glaive operations"
                />
            }
        >
            <FluentDashPages />
        </SharedAppFrame>
    );
}