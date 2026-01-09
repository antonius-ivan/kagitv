import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import './index.css'
//import App from './App.tsx'
import { FluentProvider, webLightTheme } from '@fluentui/react-components'
//import RealWeatherForecast from './pages/RealWeatherForecast.tsx'
import WebApp from './WebApp.tsx'

createRoot(document.getElementById('root')!).render(
    <StrictMode>
        <FluentProvider theme={webLightTheme}>
            <WebApp />
        </FluentProvider>,
  </StrictMode>,
)
