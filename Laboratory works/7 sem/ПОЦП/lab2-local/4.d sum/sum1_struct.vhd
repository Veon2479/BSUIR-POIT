library IEEE;
use IEEE.STD_LOGIC_1164.ALL;

entity Sum1_struct is
    Port (
            A, B, ICF : in std_logic;
            S, CF : out std_logic
         );
end Sum1_struct;

architecture Structural of Sum1_struct is

    Component And2 is
        Port (
            pin1 : in std_logic;
            pin2 : in std_logic;
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

    Component Inv1 is
        Port (
            pin : in std_logic;
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

    Component Or2 is
        Port (
            pin1 : in std_logic;
            pin2 : in std_logic;
            pout : out std_logic
         );
    end Component;

    signal AND_AB, AND_AICF, AND_BICF, OR_ABICF, AND_ABICF, IN_CF, NCF, AND_ABICFNCF : std_logic;

begin

    and2_ab: AND2 port map (A, B, AND_AB);
    and2_aicf: AND2 port map (A, ICF, AND_AICF);
    and2_bicf: AND2 port map (B, ICF, AND_BICF);
    or3_abicf: OR3 port map (A, B, ICF, OR_ABICF);
    and3_abicf: AND3 port map (A, B, ICF, AND_ABICF);

    or3_cf: OR3 port map (AND_AB, AND_AICF, AND_BICF, IN_CF);

    CF <= IN_CF;

    inv1_cf: INV1 port map (IN_CF, NCF);

    and2_abicfncf: AND2 port map (NCF, OR_ABICF, AND_ABICFNCF);

    or2_res: OR2 port map (AND_ABICFNCF, AND_ABICF, S);


end Structural;
