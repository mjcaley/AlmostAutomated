import { Record } from "../AlmostAutomated.Web/src/fable_modules/fable-library.4.0.0-theta-018/Types.js";
import { record_type, string_type, int64_type } from "../AlmostAutomated.Web/src/fable_modules/fable-library.4.0.0-theta-018/Reflection.js";

export class TemplateDTO extends Record {
    constructor(Id, Title, Description) {
        super();
        this.Id = Id;
        this.Title = Title;
        this.Description = Description;
    }
}

export function TemplateDTO$reflection() {
    return record_type("AlmostAutomated.Core.DTO.TemplateDTO", [], TemplateDTO, () => [["Id", int64_type], ["Title", string_type], ["Description", string_type]]);
}

export function toTemplateAndDetails(templateAndDetails_, templateAndDetails__1) {
    const templateAndDetails = [templateAndDetails_, templateAndDetails__1];
    const template = templateAndDetails[0];
    const details = templateAndDetails[1];
    return new TemplateDTO(template.Id, details.Title, details.Description);
}

