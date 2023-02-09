import { Record } from "../../AlmostAutomated.Web/src/fable_modules/fable-library.3.7.20/Types.js";
import { record_type, option_type, class_type } from "../../AlmostAutomated.Web/src/fable_modules/fable-library.3.7.20/Reflection.js";

export class Select extends Record {
    constructor(Id, Created, Deleted) {
        super();
        this.Id = Id;
        this.Created = Created;
        this.Deleted = Deleted;
    }
}

export function Select$reflection() {
    return record_type("AlmostAutomated.Core.Entities.Template.Select", [], Select, () => [["Id", class_type("System.Int64")], ["Created", class_type("System.DateTime")], ["Deleted", option_type(class_type("System.DateTime"))]]);
}

export class Insert extends Record {
    constructor(Created) {
        super();
        this.Created = Created;
    }
}

export function Insert$reflection() {
    return record_type("AlmostAutomated.Core.Entities.Template.Insert", [], Insert, () => [["Created", class_type("System.DateTime")]]);
}

