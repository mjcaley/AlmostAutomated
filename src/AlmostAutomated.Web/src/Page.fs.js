import { createElement } from "react";
import React from "react";
import { Header } from "./Components/Header.fs.js";
import { createObj } from "./fable_modules/fable-library.4.0.0-theta-018/Util.js";
import { Helpers_combineClasses } from "./fable_modules/Feliz.Bulma.3.0.0-pre-002/./ElementBuilders.fs.js";
import { join } from "./fable_modules/fable-library.4.0.0-theta-018/String.js";
import { Sidebar } from "./Components/Sidebar.fs.js";
import { Interop_reactApi } from "./fable_modules/Feliz.2.4.0/./Interop.fs.js";
import { ofArray } from "./fable_modules/fable-library.4.0.0-theta-018/List.js";

export function Page(pageInputProps) {
    let elms_1, elems;
    const content = pageInputProps.content;
    const children_2 = ofArray([createElement(Header, null), (elms_1 = ofArray([createElement("div", createObj(Helpers_combineClasses("column", ofArray([["className", join(" ", ["is-one-fifth"])], (elems = [createElement(Sidebar, null)], ["children", Interop_reactApi.Children.toArray(Array.from(elems))])])))), createElement("div", {
        className: "content",
        children: Interop_reactApi.Children.toArray([content]),
    })]), createElement("div", {
        className: "columns",
        children: Interop_reactApi.Children.toArray(Array.from(elms_1)),
    }))]);
    return createElement("div", {
        children: Interop_reactApi.Children.toArray(Array.from(children_2)),
    });
}

