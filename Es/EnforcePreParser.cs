// /*******************************************************
//  * Copyright (C) 2021-2022 Ryann (Elijah Cyr) <elijahcyr@protonmail.com>
//  *
//  *
//  * PakExplorer can not be copied and/or distributed without the express
//  * permission of Ryann
//  *******************************************************/

using PakExplorer.Es.Antlr;
using PakExplorer.Es.Models;

namespace PakExplorer.Es; 

public class EnforcePreParser : EnforceParserBaseListener {
    public EnforceFile EsFile;
    
    public override void ExitComputationalUnit(EnforceParser.ComputationalUnitContext context) {
        EsFile = new EnforceFile(context);
        base.ExitComputationalUnit(context);
    }
}