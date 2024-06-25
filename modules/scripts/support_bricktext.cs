
datablock StaticShapeData(BrickTextEmptyShape)
{
        shapefile = "base/data/shapes/empty.dts";
};


function fxDtsBrick::BrickText(%this,%name,%color,%distance,%client)

{
        %player = %client.player;
        %color = getColorIDTable(%color); // Better way for Colors

        if(isFunction("FilterVariableString")) 
        %name = filterVariableString(%name,%this,%client,%player);

        if(isObject(%this.textShape))
        {
                %this.textShape.setShapeName(%name);
                %this.textShape.setShapeNameColor(%color);        
                %this.textShape.setShapeNameDistance(%distance);
                %this.bricktext = %name;
        }

        else
        {
                %this.textShape = new StaticShape()
                {
                        datablock = BrickTextEmptyShape;
                        position = vectorAdd(%this.getPosition(),"0 0" SPC %this.getDatablock().brickSizeZ/9 + "0.166");
                        scale = "0.1 0.1 0.1";
                };
                %this.textShape.setShapeName(%name);
                %this.textShape.setShapeNameColor(%color);        
                %this.textShape.setShapeNameDistance(%distance);
                %this.bricktext = %name;
        }

}


datablock StaticShapeData(BrickTextEmptyShape)
{
        shapefile = "base/data/shapes/empty.dts";
};

function fxDtsBrick::BrickTextScroll(%this,%name,%color,%distance,%timer,%client)

{
        %name2 = %name;
        %amount = strLen(%name2);
        %amount2 = %amount++;

        for(%i = 0; %i <= %amount2; %i++)
        {
                if(%i == %amount2)
                {

                        %name3 = " ";

                        %time = %i * %timer;

                        %i2 = %i + 1;

                        %time2 = %i2 * %timer;

                        schedule(%time, 0, BrickTextScrollDo, %this, %name3, %color, %distance,%client);

                        schedule(%time2, 0, BrickTextScroll, %this, %name, %color, %distance, %time,%client);

                }

                else

                {

                        %letter = getSubStr(%name2, %i, 1);

                        %name3 = %name3 @ %letter;

                        %time = %i * %timer;

                        schedule(%time, 0, BrickTextScrollDo, %this, %name3, %color, %distance,%client);

                }

        }

}


function BrickTextScrollDo(%this, %name, %color, %distance,%client)

{

        %player = %client.player;

        //%Color = "getColorIDTable";

        %color = getColorIDTable(%color); // Better way for Colors

        

        if(isFunction("FilterVariableString"))

        {

            %name = filterVariableString(%name,%this,%client,%player);

        }

        

        if(isObject(%this.textShape))

        {

                %this.textShape.setShapeName(%name);

                %this.textShape.setShapeNameColor(%color);        

                %this.textShape.setShapeNameDistance(%distance);

                %this.bricktext = %name;

        }

        else

        {

                %this.textShape = new StaticShape()

                {

                        datablock = BrickTextEmptyShape;

                        position = vectorAdd(%this.getPosition(),"0 0" SPC %this.getDatablock().brickSizeZ/9 + "0.166");

                        scale = "0.1 0.1 0.1";

                };

                %this.textShape.setShapeName(%name);

                %this.textShape.setShapeNameColor(%color);        

                %this.textShape.setShapeNameDistance(%distance);

        }

}



package BrickText

{

        function fxDtsBrick::onDeath(%this)

        {

                if(isObject(%this.textShape))

                        %this.textShape.delete();

                Parent::onDeath(%this);        

        }

        

        function fxDtsBrick::onRemove(%this)

        {

                if(isObject(%this.textShape))

                        %this.textShape.delete();

                Parent::onRemove(%this);

        }

};

ActivatePackage(BrickText);
