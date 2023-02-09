import { createElement } from "react";
import React from "react";
import { createObj } from "../fable_modules/fable-library.4.0.0-theta-018/Util.js";
import { Helpers_combineClasses } from "../fable_modules/Feliz.Bulma.3.0.0-pre-002/./ElementBuilders.fs.js";
import { empty } from "../fable_modules/fable-library.4.0.0-theta-018/List.js";

export function Index() {
    return createElement("div", createObj(Helpers_combineClasses("content", empty())));
}

