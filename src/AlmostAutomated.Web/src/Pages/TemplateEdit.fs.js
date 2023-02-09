import { createElement } from "react";
import React from "react";
import { useReact_useCallback_1CA17B65, useReact_useEffectOnce_3A5B6456, useFeliz_React__React_useState_Static_1505 } from "../fable_modules/Feliz.2.4.0/React.fs.js";
import { ofArray, map, singleton, empty } from "../fable_modules/fable-library.4.0.0-theta-018/List.js";
import { PromiseBuilder__Delay_62FBFDE1, PromiseBuilder__Run_212F1D4B } from "../fable_modules/Fable.Promise.2.0.0/Promise.fs.js";
import { promise } from "../fable_modules/Fable.Promise.2.0.0/PromiseImpl.fs.js";
import { listTemplates } from "../ApiClient.fs.js";
import { some } from "../fable_modules/fable-library.4.0.0-theta-018/Option.js";
import { TemplateComponent } from "../Components/TemplateComponent.fs.js";
import { Interop_reactApi } from "../fable_modules/Feliz.2.4.0/./Interop.fs.js";
import { createObj } from "../fable_modules/fable-library.4.0.0-theta-018/Util.js";
import { Helpers_combineClasses } from "../fable_modules/Feliz.Bulma.3.0.0-pre-002/./ElementBuilders.fs.js";

export function TemplateEdit(templateEditInputProps) {
    let elms;
    const id = templateEditInputProps.id;
    const patternInput = useFeliz_React__React_useState_Static_1505(empty());
    const templates = patternInput[0];
    const setTemplates = patternInput[1];
    const fetchData = () => PromiseBuilder__Run_212F1D4B(promise, PromiseBuilder__Delay_62FBFDE1(promise, () => (listTemplates().then((_arg) => {
        const templatesResponse = _arg;
        switch (templatesResponse.tag) {
            case 2: {
                const statusCode = templatesResponse.fields[0] | 0;
                const message = templatesResponse.fields[1];
                console.error(some(`Http error ${statusCode} - ${message}`));
                return Promise.resolve();
            }
            case 1: {
                console.error(some("Error decoding templates"));
                return Promise.resolve();
            }
            case 0: {
                const t = templatesResponse.fields[0];
                setTemplates(t);
                return Promise.resolve();
            }
            case 3: {
                const exc = templatesResponse.fields[0];
                console.error(some("Some kind of network error"), singleton(exc.message));
                return Promise.resolve();
            }
            default: {
                throw new Error("Match failure: AlmostAutomated.Web.ApiClient.ApiResponse`1");
            }
        }
    }))));
    useReact_useEffectOnce_3A5B6456(useReact_useCallback_1CA17B65(() => {
        fetchData();
    }));
    const elms_1 = ofArray([(elms = map((template) => createElement(TemplateComponent, template), templates), createElement("div", {
        className: "tile",
        children: Interop_reactApi.Children.toArray(Array.from(elms)),
    })), createElement("div", createObj(Helpers_combineClasses("tile", singleton(["children", "+"]))))]);
    return createElement("div", {
        className: "block",
        children: Interop_reactApi.Children.toArray(Array.from(elms_1)),
    });
}

