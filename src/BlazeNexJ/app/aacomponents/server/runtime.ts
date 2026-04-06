import "server-only";

export function extractHttpUrl(value: unknown): string | null {
  if (typeof value === "string") {
    const match = value.trim().match(/https?:\/\/[^\s",;]+/i);
    return match ? match[0] : null;
  }

  if (Array.isArray(value)) {
    for (const item of value) {
      const extracted = extractHttpUrl(item);

      if (extracted) {
        return extracted;
      }
    }

    return null;
  }

  if (value && typeof value === "object") {
    for (const nestedValue of Object.values(value)) {
      const extracted = extractHttpUrl(nestedValue);

      if (extracted) {
        return extracted;
      }
    }
  }

  return null;
}

export function normalizeEndpointCandidate(value: string | undefined | null) {
  if (!value) {
    return null;
  }

  const trimmedValue = value.trim();

  if (!trimmedValue) {
    return null;
  }

  const directUrl = extractHttpUrl(trimmedValue);

  if (directUrl) {
    return directUrl.replace(/\/+$/, "");
  }

  if (trimmedValue.startsWith("{") || trimmedValue.startsWith("[")) {
    try {
      const parsedValue = JSON.parse(trimmedValue);
      const parsedUrl = extractHttpUrl(parsedValue);

      if (parsedUrl) {
        return parsedUrl.replace(/\/+$/, "");
      }
    } catch {
      return null;
    }
  }

  return null;
}

export function resolveServiceBaseUrl(
  explicitCandidates: Array<string | undefined>,
  servicePattern: RegExp
) {
  const discoveredCandidates = Object.entries(process.env)
    .filter(([key, value]) => {
      return Boolean(value && servicePattern.test(key) && /(http|https|url|uri|endpoint)/i.test(key));
    })
    .map(([, value]) => value);

  return [...explicitCandidates, ...discoveredCandidates]
    .map((candidate) => normalizeEndpointCandidate(candidate))
    .find((candidate) => candidate) ?? null;
}