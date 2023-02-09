import { createElement } from "react";
import React from "react";
import { createObj } from "../fable_modules/fable-library.4.0.0-theta-018/Util.js";
import { Helpers_combineClasses } from "../fable_modules/Feliz.Bulma.3.0.0-pre-002/./ElementBuilders.fs.js";
import { singleton, ofArray } from "../fable_modules/fable-library.4.0.0-theta-018/List.js";
import { Interop_reactApi } from "../fable_modules/Feliz.2.4.0/./Interop.fs.js";

export function Header() {
    let elms;
    const elms_1 = singleton((elms = singleton(createElement("h1", createObj(Helpers_combineClasses("title", ofArray([["children", "Almost Automated"], ["className", "is-primary"]]))))), createElement("div", {
        className: "hero-body",
        children: Interop_reactApi.Children.toArray(Array.from(elms)),
    })));
    return createElement("section", {
        className: "hero",
        children: Interop_reactApi.Children.toArray(Array.from(elms_1)),
    });
}

