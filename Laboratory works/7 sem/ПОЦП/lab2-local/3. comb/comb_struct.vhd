library IEEE;
use IEEE.STD_LOGIC_1164.ALL;
use IEEE.NUMERIC_STD.ALL;

--var 10

entity Comb_struct is
    Port (
            X, Y, Z : in std_logic;
            F : out std_logic
         );
end Comb_struct;

architecture Structural of Comb_struct is
    Component Inv1 is
        Port (
            pin : in std_logic;
            pout : out std_logic
         );
    end Component;

    Component And2 is
        Port (
            pin1 : in std_logic;
            pin2 : in std_logic;
            pout : out std_logic
         );
    end Component;

    Component Or2 is
        Port (
            pin1 : in std_logic;
            pin2 : in std_logic;
            pout : out std_logic
         );
    end Component;

    Component Or3 is
        Port (
            pin1 : in std_logic;
            pin2 : in std_logic;
            pin3 : in std_logic;
            pout : out std_logic
         );
    end Component;

    Component And3 is
        Port (
            pin1 : in std_logic;
            pin2 : in std_logic;
            pin3 : in std_logic;
            pout : out std_logic
         );
    end Component;


    signal NX, NY, NZ, XZ, YZ, XYZ : std_logic;

begin

    invx: Inv1 port map
    (
        pin => X,
        pout => NX
    );

    invy: Inv1 port map
    (
        pin => Y,
        pout => NY
    );

    invz: Inv1 port map
    (
        pin => Z,
        pout => NZ
    );

    andxz: And2 port map
    (
        pin1 => X,
        pin2 => Z,
        pout => XZ
    );

    andyz: And2 port map
    (
        pin1 => NY,
        pin2 => Z,
        pout => YZ
    );

    andxyz: And3 port map
    (
        pin1 => NX,
        pin2 => Y,
        pin3 => NZ,
        pout => XYZ
    );

    orres: Or3 port map
    (
        pin1 => XZ,
        pin2 => YZ,
        pin3 => XYZ,
        pout => F
    );

end Structural;
