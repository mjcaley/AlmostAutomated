module BulmaExtensions

[<Literal>]
let ``size 3/4`` = "is-three-quarters"

[<Literal>]
let ``size 2/3`` = "is-two-thirds"

[<Literal>]
let ``size 1/2`` = "is-half"

[<Literal>]
let ``size 1/3`` = "is-one-third"

[<Literal>]
let ``size 1/4`` = "is-one-quarter"

[<Literal>]
let ``size full`` = "is-full"

[<Literal>]
let ``size 4/5`` = "is-four-fifths"

[<Literal>]
let ``size 3/5`` = "is-three-fifths"

[<Literal>]
let ``size 2/5`` = "is-two-fifths"

[<Literal>]
let ``size 1/5`` = "is-one-fifth"

[<Literal>]
let ``size 1`` = "is-1"

[<Literal>]
let ``size 2`` = "is-2"

[<Literal>]
let ``size 3`` = "is-3"

[<Literal>]
let ``size 4`` = "is-4"

[<Literal>]
let ``size 5`` = "is-5"

[<Literal>]
let ``size 6`` = "is-6"

[<Literal>]
let ``size 7`` = "is-7"

let [<Literal>] ``size 8`` = "is-8"
let [<Literal>] ``size 9`` = "is-9"
let [<Literal>] ``size 10`` = "is-10"
let [<Literal>] ``size 11`` = "is-11"
let [<Literal>] ``size 12`` = "is-12"
let [<Literal>] ``size narrow`` = "is-narrow"

type sizeExt =
    static member isThreeQuarters = ``size 3/4``
