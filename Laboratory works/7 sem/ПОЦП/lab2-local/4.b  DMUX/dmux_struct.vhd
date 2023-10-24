library IEEE;
use IEEE.STD_LOGIC_1164.ALL;
use IEEE.NUMERIC_STD.ALL;

entity Dmux_struct is
    Port (
            A, S0, S1 : in std_logic;
            B0, B1, B2, B3 : out std_logic
         );

end Dmux_struct;

architecture Structural of Dmux_struct is

    Component Inv1 is
        Port (
                pin : in std_logic;
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

    signal NS0, NS1 : std_logic;

begin

    invS0: Inv1 port map(pin => S0, pout => NS0);
    invS1: Inv1 port map(pin => S1, pout => NS1);

    andB0: And3 port map(pin1 => NS0, pin2 => NS1, pin3 => A, pout => B0);
    andB1: And3 port map(pin1 => NS0, pin2 => S1, pin3 => A, pout => B1);
    andB2: And3 port map(pin1 => S0, pin2 => NS1, pin3 => A, pout => B2);
    andB3: And3 port map(pin1 => S0, pin2 => S1, pin3 => A, pout => B3);

end Structural;
