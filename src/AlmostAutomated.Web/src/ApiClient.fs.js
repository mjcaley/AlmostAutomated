import { Union } from "./fable_modules/fable-library.4.0.0-theta-018/Types.js";
import { unit_type, equals, array_type, obj_type, union_type, class_type, string_type, int32_type } from "./fable_modules/fable-library.4.0.0-theta-018/Reflection.js";
import { join } from "./fable_modules/fable-library.4.0.0-theta-018/String.js";
import { newGuid } from "./fable_modules/fable-library.4.0.0-theta-018/Guid.js";
import { add } from "./fable_modules/fable-library.4.0.0-theta-018/Map.js";
import { Auto_generateBoxedEncoderCached_437914C6, int64 } from "./fable_modules/Thoth.Json.9.1.0/./Encode.fs.js";
import { Auto_generateBoxedDecoderCached_Z6670B51, int64 as int64_1 } from "./fable_modules/Thoth.Json.9.1.0/./Decode.fs.js";
import { empty } from "./fable_modules/Thoth.Json.9.1.0/Extra.fs.js";
import { ExtraCoders } from "./fable_modules/Thoth.Json.9.1.0/Types.fs.js";
import { PromiseBuilder__Delay_62FBFDE1, PromiseBuilder__Run_212F1D4B } from "./fable_modules/Fable.Promise.2.0.0/Promise.fs.js";
import { promise } from "./fable_modules/Fable.Promise.2.0.0/PromiseImpl.fs.js";
import { PromiseBuilder__Delay_62FBFDE1 as PromiseBuilder__Delay_62FBFDE1_1, PromiseBuilder__Run_212F1D4B as PromiseBuilder__Run_212F1D4B_1 } from "./fable_modules/Thoth.Fetch.3.0.1/../Fable.Promise.2.0.0/Promise.fs.js";
import { promise as promise_1 } from "./fable_modules/Thoth.Fetch.3.0.1/../Fable.Promise.2.0.0/PromiseImpl.fs.js";
import { FSharpResult$2 } from "./fable_modules/fable-library.4.0.0-theta-018/Choice.js";
import { FetchError } from "./fable_modules/Thoth.Fetch.3.0.1/Fetch.fs.js";
import { Helper_fetch, Helper_withContentTypeJson, Helper_withProperties } from "./fable_modules/Thoth.Fetch.3.0.1/./Fetch.fs.js";
import { Types_RequestProperties } from "./fable_modules/Fable.Fetch.2.1.0/Fetch.fs.js";
import { keyValueList } from "./fable_modules/fable-library.4.0.0-theta-018/MapUtil.js";
import { cons, ofArray, empty as empty_1 } from "./fable_modules/fable-library.4.0.0-theta-018/List.js";
import { map, defaultArg } from "./fable_modules/fable-library.4.0.0-theta-018/Option.js";
import { toString } from "./fable_modules/Thoth.Fetch.3.0.1/../Thoth.Json.9.1.0/Encode.fs.js";
import { TemplateDTO$reflection } from "../../AlmostAutomated.Core/DTO.fs.js";
import { fromString } from "./fable_modules/Thoth.Fetch.3.0.1/../Thoth.Json.9.1.0/Decode.fs.js";
import { uncurry } from "./fable_modules/fable-library.4.0.0-theta-018/Util.js";

export class ApiResponse$1 extends Union {
    constructor(tag, fields) {
        super();
        this.tag = tag;
        this.fields = fields;
    }
    cases() {
        return ["Success", "DecodeError", "HttpError", "NetworkErr", "UnknownError"];
    }
}

export function ApiResponse$1$reflection(gen0) {
    return union_type("AlmostAutomated.Web.ApiClient.ApiResponse`1", [gen0], ApiResponse$1, () => [[["Item", gen0]], [], [["Item1", int32_type], ["Item2", string_type]], [["Item", class_type("System.Exception")]], []]);
}

export function formatApiUrl(apiBase_1, paths) {
    return apiBase_1 + join("/", paths);
}

export const apiBase = process.env["API_ROOT"] ? process.env["API_ROOT"] : '';

export const extras = new ExtraCoders((() => {
    let copyOfStruct = newGuid();
    return copyOfStruct;
})(), add("System.Int64", [int64, int64_1], empty.Coders));

export function listTemplates() {
    return PromiseBuilder__Run_212F1D4B(promise, PromiseBuilder__Delay_62FBFDE1(promise, () => {
        let url, data_1, caseStrategy_1, extra_1;
        return ((url = formatApiUrl(apiBase, ["templates"]), (data_1 = (void 0), (caseStrategy_1 = (void 0), (extra_1 = extras, (() => {
            let properties_2;
            try {
                const properties_3 = Helper_withProperties(void 0, (properties_2 = ofArray([new Types_RequestProperties(0, ["GET"]), new Types_RequestProperties(1, [keyValueList(Helper_withContentTypeJson(data_1, empty_1()), 0)])]), defaultArg(map((data_1_1) => {
                    let encoder;
                    return cons(new Types_RequestProperties(2, [(encoder = Auto_generateBoxedEncoderCached_437914C6(obj_type, caseStrategy_1, extra_1), toString(0, encoder(data_1_1)))]), properties_2);
                }, data_1), properties_2)));
                const pr = PromiseBuilder__Run_212F1D4B_1(promise_1, PromiseBuilder__Delay_62FBFDE1_1(promise_1, () => (Helper_fetch(url, properties_3).then((_arg) => {
                    let response_1, decoder_1_1, decode;
                    const response = _arg;
                    return ((response_1 = response, (decoder_1_1 = defaultArg(void 0, Auto_generateBoxedDecoderCached_Z6670B51(array_type(TemplateDTO$reflection()), caseStrategy_1, extra_1)), (decode = ((body_1) => fromString(uncurry(2, decoder_1_1), body_1)), PromiseBuilder__Run_212F1D4B_1(promise_1, PromiseBuilder__Delay_62FBFDE1_1(promise_1, () => (((response_1.ok) ? PromiseBuilder__Run_212F1D4B_1(promise_1, PromiseBuilder__Delay_62FBFDE1_1(promise_1, () => (response_1.text().then((_arg_1) => {
                        let matchValue, msg, value_1_1;
                        const body_1_1 = _arg_1;
                        return Promise.resolve(equals(array_type(TemplateDTO$reflection()), unit_type) ? (new FSharpResult$2(0, [void 0])) : ((matchValue = decode(body_1_1), (matchValue.tag === 1) ? ((msg = matchValue.fields[0], new FSharpResult$2(1, [new FetchError(1, [msg])]))) : ((value_1_1 = matchValue.fields[0], new FSharpResult$2(0, [value_1_1]))))));
                    })))) : (Promise.resolve(new FSharpResult$2(1, [new FetchError(2, [response_1])])))).then((_arg_1_1) => {
                        const result = _arg_1_1;
                        return Promise.resolve(result);
                    }))))))));
                }))));
                return pr.then(void 0, ((arg_3) => (new FSharpResult$2(1, [new FetchError(3, [arg_3])]))));
            }
            catch (exn) {
                return PromiseBuilder__Run_212F1D4B_1(promise_1, PromiseBuilder__Delay_62FBFDE1_1(promise_1, () => (Promise.resolve(new FSharpResult$2(1, [new FetchError(0, [exn])])))));
            }
        })()))))).then((_arg_2) => {
            let message, response_2, exc, t;
            const templateResult = _arg_2;
            return Promise.resolve((templateResult.tag === 1) ? ((message = templateResult.fields[0], (message.tag === 1) ? (new ApiResponse$1(1, [])) : ((message.tag === 2) ? ((response_2 = message.fields[0], new ApiResponse$1(2, [response_2.status, response_2.statusText]))) : ((message.tag === 3) ? ((exc = message.fields[0], new ApiResponse$1(3, [exc]))) : (new ApiResponse$1(4, [])))))) : ((t = templateResult.fields[0], new ApiResponse$1(0, [ofArray(t)]))));
        });
    }));
}

export function getTemplate(id) {
    return PromiseBuilder__Run_212F1D4B(promise, PromiseBuilder__Delay_62FBFDE1(promise, () => {
        let url, data_1, caseStrategy_1, extra_1;
        return ((url = formatApiUrl(apiBase, ["template", id]), (data_1 = (void 0), (caseStrategy_1 = (void 0), (extra_1 = extras, (() => {
            let properties_2;
            try {
                const properties_3 = Helper_withProperties(void 0, (properties_2 = ofArray([new Types_RequestProperties(0, ["GET"]), new Types_RequestProperties(1, [keyValueList(Helper_withContentTypeJson(data_1, empty_1()), 0)])]), defaultArg(map((data_1_1) => {
                    let encoder;
                    return cons(new Types_RequestProperties(2, [(encoder = Auto_generateBoxedEncoderCached_437914C6(obj_type, caseStrategy_1, extra_1), toString(0, encoder(data_1_1)))]), properties_2);
                }, data_1), properties_2)));
                const pr = PromiseBuilder__Run_212F1D4B_1(promise_1, PromiseBuilder__Delay_62FBFDE1_1(promise_1, () => (Helper_fetch(url, properties_3).then((_arg) => {
                    let response_1, decoder_1_1, decode;
                    const response = _arg;
                    return ((response_1 = response, (decoder_1_1 = defaultArg(void 0, Auto_generateBoxedDecoderCached_Z6670B51(TemplateDTO$reflection(), caseStrategy_1, extra_1)), (decode = ((body_1) => fromString(uncurry(2, decoder_1_1), body_1)), PromiseBuilder__Run_212F1D4B_1(promise_1, PromiseBuilder__Delay_62FBFDE1_1(promise_1, () => (((response_1.ok) ? PromiseBuilder__Run_212F1D4B_1(promise_1, PromiseBuilder__Delay_62FBFDE1_1(promise_1, () => (response_1.text().then((_arg_1) => {
                        let matchValue, msg, value_1_1;
                        const body_1_1 = _arg_1;
                        return Promise.resolve(equals(TemplateDTO$reflection(), unit_type) ? (new FSharpResult$2(0, [void 0])) : ((matchValue = decode(body_1_1), (matchValue.tag === 1) ? ((msg = matchValue.fields[0], new FSharpResult$2(1, [new FetchError(1, [msg])]))) : ((value_1_1 = matchValue.fields[0], new FSharpResult$2(0, [value_1_1]))))));
                    })))) : (Promise.resolve(new FSharpResult$2(1, [new FetchError(2, [response_1])])))).then((_arg_1_1) => {
                        const result = _arg_1_1;
                        return Promise.resolve(result);
                    }))))))));
                }))));
                return pr.then(void 0, ((arg_3) => (new FSharpResult$2(1, [new FetchError(3, [arg_3])]))));
            }
            catch (exn) {
                return PromiseBuilder__Run_212F1D4B_1(promise_1, PromiseBuilder__Delay_62FBFDE1_1(promise_1, () => (Promise.resolve(new FSharpResult$2(1, [new FetchError(0, [exn])])))));
            }
        })()))))).then((_arg_2) => {
            let message, response_2, exc, t;
            const templateResult = _arg_2;
            return Promise.resolve((templateResult.tag === 1) ? ((message = templateResult.fields[0], (message.tag === 1) ? (new ApiResponse$1(1, [])) : ((message.tag === 2) ? ((response_2 = message.fields[0], new ApiResponse$1(2, [response_2.status, response_2.statusText]))) : ((message.tag === 3) ? ((exc = message.fields[0], new ApiResponse$1(3, [exc]))) : (new ApiResponse$1(4, [])))))) : ((t = templateResult.fields[0], new ApiResponse$1(0, [t]))));
        });
    }));
}

