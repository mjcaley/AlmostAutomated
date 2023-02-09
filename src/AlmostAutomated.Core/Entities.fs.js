import { Record } from "../AlmostAutomated.Web/src/fable_modules/fable-library.4.0.0-theta-018/Types.js";
import { string_type, record_type, option_type, class_type, int64_type } from "../AlmostAutomated.Web/src/fable_modules/fable-library.4.0.0-theta-018/Reflection.js";

export class Template_Select extends Record {
    constructor(Id, Created, Deleted) {
        super();
        this.Id = Id;
        this.Created = Created;
        this.Deleted = Deleted;
    }
}

export function Template_Select$reflection() {
    return record_type("AlmostAutomated.Core.Entities.Template.Select", [], Template_Select, () => [["Id", int64_type], ["Created", class_type("System.DateTime")], ["Deleted", option_type(class_type("System.DateTime"))]]);
}

export class Template_Insert extends Record {
    constructor(Created) {
        super();
        this.Created = Created;
    }
}

export function Template_Insert$reflection() {
    return record_type("AlmostAutomated.Core.Entities.Template.Insert", [], Template_Insert, () => [["Created", class_type("System.DateTime")]]);
}

export class TemplateDetails_Select extends Record {
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

export function TemplateDetails_Select$reflection() {
    return record_type("AlmostAutomated.Core.Entities.TemplateDetails.Select", [], TemplateDetails_Select, () => [["Id", int64_type], ["Title", string_type], ["Description", string_type], ["ValidFrom", class_type("System.DateTime")], ["ValidTo", option_type(class_type("System.DateTime"))], ["TemplateId", int64_type]]);
}

export class TemplateDetails_Insert extends Record {
    constructor(Title, Description, TemplateId) {
        super();
        this.Title = Title;
        this.Description = Description;
        this.TemplateId = TemplateId;
    }
}

export function TemplateDetails_Insert$reflection() {
    return record_type("AlmostAutomated.Core.Entities.TemplateDetails.Insert", [], TemplateDetails_Insert, () => [["Title", string_type], ["Description", string_type], ["TemplateId", int64_type]]);
}

export class TemplateDetails_Insert$0027 extends Record {
    constructor(Title, Description) {
        super();
        this.Title = Title;
        this.Description = Description;
    }
}

export function TemplateDetails_Insert$0027$reflection() {
    return record_type("AlmostAutomated.Core.Entities.TemplateDetails.Insert\'", [], TemplateDetails_Insert$0027, () => [["Title", string_type], ["Description", string_type]]);
}

