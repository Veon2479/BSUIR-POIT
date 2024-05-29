----------------------------------------------------------------------------------
-- Company: 
-- Engineer: 
-- 
-- Create Date: 10/30/2023 05:45:35 PM
-- Design Name: 
-- Module Name: Bistable - Behavioral
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
library UNISIM;
use UNISIM.VComponents.all;

entity Bistable is

    Port (
        Q   : out std_logic;
        NQ  : out std_logic
    );

end Bistable;

architecture Struct of Bistable is

    attribute dont_touch : string;
    attribute dont_touch of Struct : architecture is "true";

    signal q_reg  : std_logic;
    signal nq_reg : std_logic;
    
    component Inv1 is
        Port (
            pin : in std_logic;
            pout : out std_logic
        );
    end component;
    
begin

    inv1_1: Inv1 port map (pin => nq_reg, pout => q_reg);
    inv1_2: Inv1 port map (pin => q_reg, pout => nq_reg);

    Q  <= q_reg;
    NQ <= nq_reg;
    
end Struct;
