import { createServer, IncomingMessage, ServerResponse } from "http";
import next from "next";
import './instrumentation.ts';

console.log('Hello world');

export { };

const port = parseInt(process.env.PORT || "3000", 10);//Random Port for server
const dev = process.env.NODE_ENV !== "production";
const createNextApp = next as unknown as (options: { dev: boolean }) => {
    prepare(): Promise<void>;
    getRequestHandler(): (req: IncomingMessage, res: ServerResponse<IncomingMessage>) => void;
};
const app = createNextApp({ dev });
const handle = app.getRequestHandler();

const debugArgPattern = /^--(?:inspect|inspect-brk)(?:=.*)?$/;
process.execArgv = process.execArgv.filter((arg) => !debugArgPattern.test(arg));

function isHealthRequest(req: IncomingMessage) {
    if (!req.url) {
        return false;
    }

    const [path] = req.url.split("?", 1);
    return path === "/health" && (req.method === "GET" || req.method === "HEAD");
}

function writeHealthResponse(res: ServerResponse<IncomingMessage>) {
    const payload = JSON.stringify({
        status: "ok",
        service: "blazenexj",
        uptimeSeconds: Math.floor(process.uptime()),
        timestampUtc: new Date().toISOString()
    });

    res.statusCode = 200;
    res.setHeader("Content-Type", "application/json; charset=utf-8");
    res.setHeader("Cache-Control", "no-store");
    res.end(payload);
}

function delay(ms: number) {
    return new Promise((resolve) => setTimeout(resolve, ms));
}

console.log(`Node process id: ${process.pid}`);
//logger.info('Node process id announced for debugger attach', { pid: process.pid });
//logger.info('Waiting for dependent services', { delayMs: 1000 });
await delay(1000);

app.prepare().then(() => {
    createServer((req, res) => {
        if (isHealthRequest(req)) {
            writeHealthResponse(res);
            return;
        }

        handle(req, res);
    }).listen(port);

    console.log(
        `> Server listening at http://localhost:${port} as ${dev ? "development" : process.env.NODE_ENV
        }`
    );
});

console.log('Hello world2');
//logger.info('Preparing Next.js', { dev, hostname, port });