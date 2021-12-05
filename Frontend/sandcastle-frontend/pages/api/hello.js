// Next.js API route support: https://nextjs.org/docs/api-routes/introduction

export default function handler(req, res) {
  res.status(200).json({
    "_id":{
       "$oid":"61abb9f63d0248abd1aeb1f2"
    },
    "Nome":"Nome",
    "Classe":"Classe",
    "Especificidades":"Especificidade",
    "Força":10,
    "Destreza":10,
    "Inteligencia":10,
    "Vigor":10
 })
}
