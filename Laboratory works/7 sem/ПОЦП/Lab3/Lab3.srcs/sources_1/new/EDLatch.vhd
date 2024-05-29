----------------------------------------------------------------------------------
-- Company: 
-- Engineer: 
-- 
-- Create Date: 11/02/2023 08:23:27 PM
-- Design Name: 
-- Module Name: EDLatch - Behavioral
-- Project Name: 
-- Target Devices: 
-- Tool Versions: 
-- Description: 
-- 
-- Dependencies: 
-- 
-- Revision:
-- Revision 0.01 - File Created
-- Additional Comments:
-- 
----------------------------------------------------------------------------------


library IEEE;
use IEEE.STD_LOGIC_1164.ALL;

-- Uncomment the following library declaration if using
-- arithmetic functions with Signed or Unsigned values
--use IEEE.NUMERIC_STD.ALL;

-- Uncomment the following library declaration if instantiating
-- any Xilinx leaf cells in this code.
--library UNISIM;
--use UNISIM.VComponents.all;

entity EDLatch is
    Port ( E : in STD_LOGIC;
           D : in STD_LOGIC;
           Q : out STD_LOGIC;
           NQ : out STD_LOGIC);
end EDLatch;

architecture Struct of EDLatch is
    Component RSLatch is
        Port ( R : in STD_LOGIC;
               S : in STD_LOGIC;
               Q : out STD_LOGIC;
               NQ : out STD_LOGIC);
    end component;
    
    Component Inv1 is 
        Port (
            pin : in std_logic;
            pout : out std_logic
        );
    end component;
    
    Component And2 is
        Port (
            pin1 : in std_logic;
            pin2 : in std_logic;
            pout : out std_logic
         );
    end component;
    
    signal ND, ED, NDE : std_logic;
begin

    inv1_0: Inv1 port map (pin=>D, pout=>ND);
    and2_0: And2 port map (pin1=>E, pin2=>D, pout=>ED);
    and2_1: And2 port map (pin1=>E, pin2=>ND, pout=>NDE);
    rs_0: RSLatch port map (R=>NDE, S=>ED, Q=>NQ, NQ=>Q);

end Struct;
