import { createElement } from "react";
import React from "react";
import { createObj } from "../fable_modules/fable-library.4.0.0-theta-018/Util.js";
import { Helpers_combineClasses } from "../fable_modules/Feliz.Bulma.3.0.0-pre-002/./ElementBuilders.fs.js";
import { toString } from "../fable_modules/fable-library.4.0.0-theta-018/Long.js";
import { singleton, ofArray } from "../fable_modules/fable-library.4.0.0-theta-018/List.js";
import { Interop_reactApi } from "../fable_modules/Feliz.2.4.0/./Interop.fs.js";

export function TemplateComponent(template) {
    let elems_3, elms_2, elms_1, elms;
    return createElement("div", createObj(Helpers_combineClasses("card", ofArray([["id", "template-" + toString(template.Id)], ["className", "is-size-7"], ["className", "mx-4"], ["className", "my-4"], (elems_3 = [(elms_2 = ofArray([(elms_1 = singleton((elms = singleton(createElement("p", createObj(Helpers_combineClasses("title", ofArray([["className", "is-4"], ["children", template.Title]]))))), createElement("div", {
        className: "media-content",
        children: Interop_reactApi.Children.toArray(Array.from(elms)),
    }))), createElement("article", {
        className: "media",
        children: Interop_reactApi.Children.toArray(Array.from(elms_1)),
    })), createElement("div", {
        className: "content",
        children: template.Description,
    })]), createElement("div", {
        className: "card-content",
        children: Interop_reactApi.Children.toArray(Array.from(elms_2)),
    }))], ["children", Interop_reactApi.Children.toArray(Array.from(elems_3))])]))));
}

