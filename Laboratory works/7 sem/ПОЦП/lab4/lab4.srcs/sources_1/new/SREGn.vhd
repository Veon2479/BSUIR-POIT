----------------------------------------------------------------------------------
-- Company: 
-- Engineer: 
-- 
-- Create Date: 11/06/2023 01:10:34 PM
-- Design Name: 
-- Module Name: SREGn - Structural
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

entity SREGn_s is
    Generic (n : integer := 4);
    Port ( Sin : in STD_LOGIC;
           Sout : out STD_LOGIC_VECTOR (0 to n-1);
           SE : in STD_LOGIC;
           Clk : in STD_LOGIC;
           Rst : in STD_LOGIC);
end SREGn_s;

architecture Structural of SREGn_s is
    signal sreg : std_logic_vector(0 to n-1);
begin
    fdff: FDCE port map (sreg(0), Clk, SE, Rst, Sin);
    DFFs: for i in 1 to n-1 generate
        DFFi: FDCE port map (sreg(i), Clk, SE, Rst, sreg(i - 1));
    end generate;
    Sout <= sreg;
end Structural;
