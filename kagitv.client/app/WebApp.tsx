import { BrowserRouter, Route, Routes } from "react-router-dom";
//import DashboardLayout from "./layouts/DashboardLayout";
import type { NavDrawerProps } from "@fluentui/react-components";
import RealWeatherForecast from "./pages/RealWeatherForecast";
//import RealWeatherForecast from "./pages/RealWeatherForecast";

export const WebApp = (props: Partial<NavDrawerProps>) => {
    return (
        <BrowserRouter>
            <Routes>
                <Route path="/" element={<RealWeatherForecast />} />
            </Routes>
        </BrowserRouter>
    );
};

export default WebApp;