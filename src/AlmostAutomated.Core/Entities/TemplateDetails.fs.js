import { Record } from "../../AlmostAutomated.Web/src/fable_modules/fable-library.3.7.20/Types.js";
import { record_type, option_type, string_type, class_type } from "../../AlmostAutomated.Web/src/fable_modules/fable-library.3.7.20/Reflection.js";

export class Select extends Record {
    constructor(Id, Title, Description, ValidFrom, ValidTo, TemplateId) {
        super();
        this.Id = Id;
        this.Title = Title;
        this.Description = Description;
        this.ValidFrom = ValidFrom;
        this.ValidTo = ValidTo;
        this.TemplateId = TemplateId;
    }
}

export function Select$reflection() {
    return record_type("AlmostAutomated.Core.Entities.TemplateDetails.Select", [], Select, () => [["Id", class_type("System.Int64")], ["Title", string_type], ["Description", string_type], ["ValidFrom", class_type("System.DateTime")], ["ValidTo", option_type(class_type("System.DateTime"))], ["TemplateId", class_type("System.Int64")]]);
}

export class Insert extends Record {
    constructor(Title, Description, TemplateId) {
        super();
        this.Title = Title;
        this.Description = Description;
        this.TemplateId = TemplateId;
    }
}

export function Insert$reflection() {
    return record_type("AlmostAutomated.Core.Entities.TemplateDetails.Insert", [], Insert, () => [["Title", string_type], ["Description", string_type], ["TemplateId", class_type("System.Int64")]]);
}

export class Insert$0027 extends Record {
    constructor(Title, Description) {
        super();
        this.Title = Title;
        this.Description = Description;
    }
}

export function Insert$0027$reflection() {
    return record_type("AlmostAutomated.Core.Entities.TemplateDetails.Insert\u0027", [], Insert$0027, () => [["Title", string_type], ["Description", string_type]]);
}

