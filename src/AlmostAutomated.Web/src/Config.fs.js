import { isNullOrWhiteSpace } from "./fable_modules/fable-library.4.0.0-theta-018/String.js";

export function variableOrDefault(key, defaultValue) {
    const foundValue = process.env[key] ? process.env[key] : '';
    if (isNullOrWhiteSpace(foundValue)) {
        return defaultValue;
    }
    else {
        return foundValue;
    }
}

