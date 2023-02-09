import { createElement } from "react";
import React from "react";
import * as react from "react";
import { Index } from "./Pages/Index.fs.js";
import { Page } from "./Page.fs.js";
import { Templates } from "./Pages/Templates.fs.js";
import { TemplateEdit } from "./Pages/TemplateEdit.fs.js";
import { Error404 } from "./Components/Error404.fs.js";
import { ofArray, singleton, tail, head, isEmpty } from "./fable_modules/fable-library.4.0.0-theta-018/List.js";
import { Route_$007CInt64$007C_$007C } from "./fable_modules/Feliz.Router.4.0.0/Router.fs.js";
import { useFeliz_React__React_useState_Static_1505 } from "./fable_modules/Feliz.2.4.0/React.fs.js";
import { RouterModule_router, RouterModule_urlSegments } from "./fable_modules/Feliz.Router.4.0.0/./Router.fs.js";
import { createObj } from "./fable_modules/fable-library.4.0.0-theta-018/Util.js";

export function routes(url) {
    let matchResult, id;
    if (!isEmpty(url)) {
        if (head(url) === "templates") {
            if (!isEmpty(tail(url))) {
                const activePatternResult = Route_$007CInt64$007C_$007C(head(tail(url)));
                if (activePatternResult != null) {
                    if (isEmpty(tail(tail(url)))) {
                        matchResult = 2;
                        id = activePatternResult;
                    }
                    else {
                        matchResult = 3;
                    }
                }
                else {
                    matchResult = 3;
                }
            }
            else {
                matchResult = 1;
            }
        }
        else {
            matchResult = 3;
        }
    }
    else {
        matchResult = 0;
    }
    switch (matchResult) {
        case 0: {
            const content = createElement(Index, null);
            return createElement(Page, {
                content: content,
            });
        }
        case 1: {
            const content_1 = createElement(Templates, null);
            return createElement(Page, {
                content: content_1,
            });
        }
        case 2: {
            const content_2 = createElement(TemplateEdit, {
                id: id,
            });
            return createElement(Page, {
                content: content_2,
            });
        }
        case 3: {
            let matchResult_1;
            if (!isEmpty(url)) {
                if (head(url) === "templates") {
                    if (!isEmpty(tail(url))) {
                        if (head(tail(url)) === "new") {
                            if (isEmpty(tail(tail(url)))) {
                                matchResult_1 = 0;
                            }
                            else {
                                matchResult_1 = 1;
                            }
                        }
                        else {
                            matchResult_1 = 1;
                        }
                    }
                    else {
                        matchResult_1 = 1;
                    }
                }
                else {
                    matchResult_1 = 1;
                }
            }
            else {
                matchResult_1 = 1;
            }
            switch (matchResult_1) {
                case 0: {
                    return createElement("h1", {
                        children: ["New template"],
                    });
                }
                case 1: {
                    const content_3 = createElement(Error404, null);
                    return createElement(Page, {
                        content: content_3,
                    });
                }
            }
        }
    }
}

export function RouterComponent() {
    let elements;
    const patternInput = useFeliz_React__React_useState_Static_1505(RouterModule_urlSegments(window.location.hash, 1));
    const updateUrl = patternInput[1];
    const currentUrl = patternInput[0];
    return RouterModule_router(createObj(ofArray([["onUrlChanged", updateUrl], (elements = singleton(routes(currentUrl)), ["application", react.createElement(react.Fragment, {}, ...elements)])])));
}

