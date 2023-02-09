import { createElement } from "react";
import React from "react";
import { singleton } from "../fable_modules/fable-library.4.0.0-theta-018/List.js";
import { Interop_reactApi } from "../fable_modules/Feliz.2.4.0/./Interop.fs.js";

export function Error404() {
    const children = singleton(createElement("h2", {
        children: ["404 Not Found"],
    }));
    return createElement("div", {
        children: Interop_reactApi.Children.toArray(Array.from(children)),
    });
}

