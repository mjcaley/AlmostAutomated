import { createElement } from "react";
import React from "react";
import * as react from "react";
import { Index } from "./Pages/Index.fs.js";
import { printf, toText } from "./fable_modules/fable-library.4.0.0-theta-018/String.js";
import { Error404 } from "./Components/Error404.fs.js";
import { ofArray, singleton, tail, head, isEmpty } from "./fable_modules/fable-library.4.0.0-theta-018/List.js";
import { Route_$007CInt$007C_$007C } from "./fable_modules/Feliz.Router.4.0.0/Router.fs.js";
import { useFeliz_React__React_useState_Static_1505 } from "./fable_modules/Feliz.2.4.0/React.fs.js";
import { RouterModule_router, RouterModule_urlSegments } from "./fable_modules/Feliz.Router.4.0.0/./Router.fs.js";
import { createObj } from "./fable_modules/fable-library.4.0.0-theta-018/Util.js";

export function routes(url) {
    let matchResult, userId;
    if (!isEmpty(url)) {
        if (head(url) === "users") {
            if (!isEmpty(tail(url))) {
                const activePatternResult = Route_$007CInt$007C_$007C(head(tail(url)));
                if (activePatternResult != null) {
                    if (isEmpty(tail(tail(url)))) {
                        matchResult = 2;
                        userId = activePatternResult;
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
            return createElement(Index, null);
        }
        case 1: {
            return createElement("h1", {
                children: ["Users page"],
            });
        }
        case 2: {
            return createElement("h1", {
                children: [toText(printf("User ID %d"))(userId)],
            });
        }
        case 3: {
            return createElement(Error404, null);
        }
    }
}

export function Router() {
    let elements;
    const patternInput = useFeliz_React__React_useState_Static_1505(RouterModule_urlSegments(window.location.hash, 1));
    const updateUrl = patternInput[1];
    const currentUrl = patternInput[0];
    return RouterModule_router(createObj(ofArray([["onUrlChanged", updateUrl], (elements = singleton(routes(currentUrl)), ["application", react.createElement(react.Fragment, {}, ...elements)])])));
}

