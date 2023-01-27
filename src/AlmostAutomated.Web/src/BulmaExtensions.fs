module BulmaExtensions

let [<Literal>] ``size 3/4`` = "is-three-quarters"
let [<Literal>] ``size 2/3`` = "is-two-thirds"
let [<Literal>] ``size 1/2`` = "is-half"
let [<Literal>] ``size 1/3`` = "is-one-third"
let [<Literal>] ``size 1/4`` = "is-one-quarter"
let [<Literal>] ``size full`` = "is-full"
let [<Literal>] ``size 4/5`` = "is-four-fifths"
let [<Literal>] ``size 3/5`` = "is-three-fifths"
let [<Literal>] ``size 2/5`` = "is-two-fifths"
let [<Literal>] ``size 1/5`` = "is-one-fifth"
let [<Literal>] ``size 1`` = "is-1"
let [<Literal>] ``size 2`` = "is-2"
let [<Literal>] ``size 3`` = "is-3"
let [<Literal>] ``size 4`` = "is-4"
let [<Literal>] ``size 5`` = "is-5"
let [<Literal>] ``size 6`` = "is-6"
let [<Literal>] ``size 7`` = "is-7"
let [<Literal>] ``size 8`` = "is-8"
let [<Literal>] ``size 9`` = "is-9"
let [<Literal>] ``size 10`` = "is-10"
let [<Literal>] ``size 11`` = "is-11"
let [<Literal>] ``size 12`` = "is-12"
let [<Literal>] ``size narrow`` = "is-narrow"

type sizeExt =
    static member isThreeQuarters = ``size 3/4``
