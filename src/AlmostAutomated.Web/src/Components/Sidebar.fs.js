import { createElement } from "react";
import React from "react";
import { RouterModule_encodeParts } from "../fable_modules/Feliz.Router.4.0.0/./Router.fs.js";
import { ofArray, singleton } from "../fable_modules/fable-library.4.0.0-theta-018/List.js";
import { Interop_reactApi } from "../fable_modules/Feliz.2.4.0/./Interop.fs.js";

export function Sidebar() {
    let elms;
    const elms_1 = singleton((elms = ofArray([createElement("a", {
        children: "Home",
        href: RouterModule_encodeParts(singleton("/"), 1),
    }), createElement("a", {
        children: "Templates",
        href: RouterModule_encodeParts(singleton("/templates"), 1),
    })]), createElement("ul", {
        className: "menu-list",
        children: Interop_reactApi.Children.toArray(Array.from(elms)),
    })));
    return createElement("aside", {
        className: "menu",
        children: Interop_reactApi.Children.toArray(Array.from(elms_1)),
    });
}

