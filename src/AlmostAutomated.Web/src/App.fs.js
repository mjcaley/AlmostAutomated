import { createRoot } from "react-dom/client";
import { createElement } from "react";
import { RouterComponent } from "./RouterComponent.fs.js";

export const root = createRoot(document.getElementById("root"));

root.render(createElement(RouterComponent, null));

