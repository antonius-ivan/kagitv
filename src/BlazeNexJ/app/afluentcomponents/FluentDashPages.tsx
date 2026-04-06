// import FluentProviderRegistry from "@/app/aacomponents/FluentProviderRegistry";
import FluentDashboardLayout from "./FluentDashboardLayout";
import FluentProviderRegistry from "./FluentProviderRegistry";
import { getRoleMenu } from "../aacomponents/server/menu";
// import FluentDashboardLayout from "@/app/aacomponents/layouts/msft/FluentDashboardLayout";

export default async function FluentDashPages() {
	const modules = await getRoleMenu();

	return (
		<FluentProviderRegistry>
			<FluentDashboardLayout modules={modules} />
		</FluentProviderRegistry>
	);
}
