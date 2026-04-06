import { env } from 'node:process';
import { NodeSDK } from '@opentelemetry/sdk-node';
import { OTLPTraceExporter } from '@opentelemetry/exporter-trace-otlp-grpc';
import { OTLPMetricExporter } from '@opentelemetry/exporter-metrics-otlp-grpc';
import { PeriodicExportingMetricReader } from '@opentelemetry/sdk-metrics';
import { HttpInstrumentation } from '@opentelemetry/instrumentation-http';
import { UndiciInstrumentation } from '@opentelemetry/instrumentation-undici';
import winston from 'winston';
import { OpenTelemetryTransportV3 } from '@opentelemetry/winston-transport';

const environment = process.env.NODE_ENV || 'development';
const otlpServer = env.OTEL_EXPORTER_OTLP_ENDPOINT;

if (otlpServer) {
  const sdk = new NodeSDK({
    traceExporter: new OTLPTraceExporter(),
    metricReader: new PeriodicExportingMetricReader({
      exportIntervalMillis: environment === 'development' ? 5000 : 10000,
      exporter: new OTLPMetricExporter()
    }),
    instrumentations: [
      new HttpInstrumentation(),
      new UndiciInstrumentation()
    ]
  });

  sdk.start();
}

export function createLogger(category = 'combust') {
  const transports: winston.transport[] = [
    new winston.transports.Console({
      format: winston.format.combine(
        winston.format.colorize(),
        winston.format.simple()
      )
    })
  ];

  if (otlpServer) {
    transports.push(new OpenTelemetryTransportV3());
  }

  return winston.createLogger({
    level: 'info',
    format: winston.format.json(),
    defaultMeta: { category },
    transports
  });
}
